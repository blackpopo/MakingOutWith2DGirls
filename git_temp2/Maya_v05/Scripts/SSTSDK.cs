using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine;

public class STTSDK 
{
    string subscription_key = "YourAPIKey";
    string region = "YourLocation";
    string location = "ja-JP";
    
    //Sampling Rate 11050*2, bitRate=16, channels = 1
   public  async UniTask<string> STT_Custom(string wavFilepath, int sampleRate, int bitRate, int channels) //EmpathでSamplingRateを変更する場合にしよう。ちょっと、重い？
    {
        var speechConfig = SpeechConfig.FromSubscription(subscription_key, region); 
        speechConfig.SpeechRecognitionLanguage = location;
        var reader = new BinaryReader(File.OpenRead(wavFilepath));
        var audioStreamFormat = AudioStreamFormat.GetWaveFormatPCM((uint)sampleRate, (byte)bitRate, (byte)channels);
        var audioInputStream = AudioInputStream.CreatePushStream(audioStreamFormat);
        var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
        var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

        byte[] readBytes;
        do
        {
            readBytes = reader.ReadBytes(1024);
            audioInputStream.Write(readBytes, readBytes.Length);
        } while (readBytes.Length > 0);

        var result = await recognizer.RecognizeOnceAsync();
        // Debug.Log($"Recognized Line : = {result.Text}");
        return result.Text;
    }
    
    public async UniTask STTBytes(byte[] readBytes, int sampleRate, int bitRate, int channels)
    {
        var speechConfig = SpeechConfig.FromSubscription(subscription_key, region);
        speechConfig.SpeechRecognitionLanguage = location;
        var audioStreamFormat = AudioStreamFormat.GetWaveFormatPCM((uint)sampleRate, (byte)bitRate, (byte)channels);
        var audioInputStream = AudioInputStream.CreatePushStream(audioStreamFormat);
        var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
        var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
        
        audioInputStream.Write(readBytes, readBytes.Length);
        
        var result = await recognizer.RecognizeOnceAsync();
        // Debug.Log($"Recognized Line : = {result.Text}");
    }

    public async UniTask<string> STT(string wavFilePath) //通常用。SamplingRate = 16000[Hz]
    {
        var speechConfig = SpeechConfig.FromSubscription(subscription_key, region); 
        speechConfig.SpeechRecognitionLanguage = location; // Speech config 直下にLocationを設定して言語を設定する。
        using var audioConfig = AudioConfig.FromWavFileInput(wavFilePath);
        using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
        var result = await recognizer.RecognizeOnceAsync();
        Debug.Log($"Recognized Line: = {result.Text}");
        return result.Text;
    }
}
