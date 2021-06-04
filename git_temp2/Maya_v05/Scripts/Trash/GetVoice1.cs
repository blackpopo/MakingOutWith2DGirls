using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetVoice1 : MonoBehaviour
{
    [SerializeField] 
    AudioSource listenSource;

    [SerializeField]
    Animator animator;

    //private fields
    private float gain = 8000.0f;
    float volume;
    float frequency;
    bool isSpeaking;
    bool isRecording;
    float bufferTimer;　//話した後の余韻の時間 or　言葉に詰まった時の時間　の計測時間
    float speakingTimer;　//実際に話している時間
    int head;
    private string SSTFileName;
    private string CSVFilePath;

    //constant values
    const int RECORD_SECONDS = 2;
    const int SAMPLING_FREQUENCY = 16000; // MSのSSTはサンプリングレートが16000Hz
    const int FFTSAMPLES = 2 << 8; //256bitのサンプルをFFTでは選んでとる。
    
    const int HEADER_SIZE = 44;
    const float RESCALE_FACTOR = 32767;
    
    const float MIN_FREQ = 100.0f; //母音のFrequency は　100Hz 以上 1400Hz以下。　ただし、子音字は異なる。
    const float MAX_FREQ = 1400.0f;
    const float MIN_VOLUME = 1.0f;
    const float BUFFER_TIME = 1.6f;　//話した後の余韻の時間 or　言葉に詰まった時の時間。
    const float SST_SPEAKING_TIME = 4.0f;

    
    //my defined class
    //
    private STTSDK sttsdk;
    private WordController wc;
    
    
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
        gain = 2000.0f;
        #endif
        
        
        
        SSTFileName = System.IO.Path.Combine(Application.persistentDataPath, 
            "SST.wav");
        
        #if UNITY_EDITOR
            SSTFileName = System.IO.Path.Combine("C:\\Users\\Atsuya\\UnityProjects", "TEMPTATION",
                "SST.wav");
        #endif

        sttsdk = new STTSDK();
        wc = new WordController();
        wc.ReadFile(); //ファイルの読み込み
        GetMic();
    }

    
    void Update()
    {
        Waiting();
    }
    
    
    
    private void GetMic()
    {
        while (Microphone.devices.Length< 1) { }
        string device = Microphone.devices[0];
        listenSource.loop = true;
        listenSource.clip = Microphone.Start(device, true, RECORD_SECONDS, SAMPLING_FREQUENCY);
        while (!(Microphone.GetPosition(device) > 0)) { }
        listenSource.Play();
    }
    
    async private void Waiting()
    { 
        CalculateVowel();
        if (MIN_FREQ < frequency && frequency < MAX_FREQ && MIN_VOLUME < volume)　//しゃべり始めの時間
        {
            isSpeaking = true;
            
            bufferTimer = 0.0f;
            speakingTimer += Time.deltaTime;
            
            //始めてレコーディングを開始したとき
            if (!isRecording)
            {
                isRecording = true;

                bufferTimer = 0.0f;
                speakingTimer = 0.0f;

                using (var fileStream = new FileStream(SSTFileName, FileMode.Create))
                {
                    byte[] headerSpace = new byte[HEADER_SIZE];
                    fileStream.Write(headerSpace, 0, headerSpace.Length);
                }
            }
        }
        else if (isSpeaking && volume > MIN_VOLUME)
        {
            //子音字をしゃべっていると判定
            bufferTimer = 0.0f;
            speakingTimer += Time.deltaTime;
        }
        else if (isSpeaking && bufferTimer < BUFFER_TIME) // 余韻の時間
        {
            bufferTimer += Time.deltaTime;
            speakingTimer += Time.deltaTime;
        }
        else
        {

            bufferTimer = 0.0f; // 後で、speakingTimerの条件処理あり！
            
            isSpeaking = false;
            if (isRecording)
            {
                isRecording = false;

                using (var fileStream = new FileStream(SSTFileName, FileMode.Open))
                {
                    WavHeaderWrite(fileStream, listenSource.clip.channels, SAMPLING_FREQUENCY);
                }


                if (speakingTimer > SST_SPEAKING_TIME)　//もし、一定時間以上話していたら、音声を認識する。
                {
                    Debug.Log("Send Message! \n Speaking time is" + speakingTimer);
                    string sentence;
                    string paramName;
                    int paramInt ;
                    sentence =  await sttsdk.STT(SSTFileName);

                    wc.CheckWord(sentence, out paramName, out paramInt); 
                    
                    if (paramInt != 0)
                    {
                        Debug.Log("Start Animation");
                        animator.SetInteger(paramName, paramInt);
                        speakingTimer = 0.0f;
                    }

                }
                AizuchiAnimation(speakingTimer);
            }

            speakingTimer = 0.0f;
        }

        int position = Microphone.GetPosition(null);
        if (position < 0 || head == position)
        {
            return;
        }

        if (isRecording)
        {
            var waves = new float[listenSource.clip.samples * listenSource.clip.channels];
            listenSource.clip.GetData(waves, 0);
            //GetDataだとrecordSeconds の間のデータがすべて入っている。
            //head と　position がおかしい？
            List<float> SSTAudioData = new List<float>();
            if (head < position)
            {
                for (int i = head; i < position; i++)
                {
                    SSTAudioData.Add(waves[i]);
                }
            }
            else
            {
                for (int i = head; i < waves.Length; i++)
                {
                    SSTAudioData.Add(waves[i]);
                }

                for (int i = 0; i < position; i++)
                {
                    SSTAudioData.Add(waves[i]);
                }
            }
            
            using (var fileStream = new FileStream(SSTFileName, FileMode.Append))
            {
                    WavBufferWrite(fileStream, SSTAudioData);
            }

        }

        head = position;
    }
     
   private void WavBufferWrite(FileStream fileStream, List<float> dataList)
    {
        foreach (float data in dataList)
        {
            Byte[] buffer = BitConverter.GetBytes((short)(data * RESCALE_FACTOR));
            fileStream.Write(buffer, 0, 2);
        }
        fileStream.Flush();
    }
 
    private void WavHeaderWrite(FileStream fileStream, int channels, int samplingFrequency)
    {
        //サンプリング数を計算
        var samples = ((int)fileStream.Length - HEADER_SIZE) / 2;
 
        fileStream.Seek(0, SeekOrigin.Begin);
 
        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);
        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);
        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);
        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);
        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);
        UInt16 _one = 1;
        Byte[] audioFormat = BitConverter.GetBytes(_one);
        fileStream.Write(audioFormat, 0, 2);
        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);
        Byte[] sampleRate = BitConverter.GetBytes(samplingFrequency);
        fileStream.Write(sampleRate, 0, 4);
        Byte[] byteRate = BitConverter.GetBytes(samplingFrequency * channels * 2);
        fileStream.Write(byteRate, 0, 4);
        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);
        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);
        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);
        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);
 
        fileStream.Flush();
    }


    private void CalculateVowel()
    {

        //ここが処理の重さ的にやばいかも？
        var max_volume = 0.0f;
        var max_index = 0;
        var total_volume = 0.0f;
        //録音時間＊サンプリング周波数の個数のデータがほしい！
        float[] temp = new float[FFTSAMPLES];
        listenSource.GetSpectrumData(temp, 0, FFTWindow.Blackman);
        for (int i = 0; i < temp.Length; i++)
        {
            if (max_volume < temp[i])
            {
                max_index = i;
                max_volume = temp[i];
            }
            total_volume += Mathf.Abs(temp[i]);
        }

        if (temp.Length > 0)
        {
            frequency = max_index * AudioSettings.outputSampleRate / 2 / temp.Length;
            volume = total_volume / temp.Length * gain;
        }
    }

    private void AizuchiAnimation(float speakingTime)
    {
        if (2.0f < speakingTime && speakingTime < 5.0f)
        {
            animator.SetInteger("aizuchi", 1);
        }

        else if (5.0f < speakingTime && speakingTime < 8.0f)
        {
            animator.SetInteger("aizuchi", 2);
        }
        else if (8.0f < speakingTime)
        {
            animator.SetInteger("aizuchi", 3);
        }
    }
}

