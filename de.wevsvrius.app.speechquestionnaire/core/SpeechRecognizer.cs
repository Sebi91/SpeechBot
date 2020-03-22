// MIT License
//
// Copyright (c) 2020 openhealthcheck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace de.wevsvrius.app.speechquestionnaire.core
{
    public class Speech
    {
        public const string API_KEY = "KEY";
        public const string REGION = "REGION";

        private static Speech _instance;
        //private SpeechConfig _config;
        private SpeechRecognizer _recognizer;
        private System.Speech.Synthesis.SpeechSynthesizer _synthesizer;


        public static Speech Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Speech();
                }
                return _instance;
            }
        }

        private Speech()
        {
            SpeechConfig config = SpeechConfig.FromSubscription(API_KEY, REGION);
            config.SpeechRecognitionLanguage = "de-DE";
            _recognizer = new SpeechRecognizer(config);

            _synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            ICollection<InstalledVoice> voices =_synthesizer.GetInstalledVoices(System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            foreach(InstalledVoice voice in voices)
            {
                _synthesizer.SelectVoice(voice.VoiceInfo.Name);
            }
            //_synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
        }

        public SpeechRecognitionResult Recognize()
        {
            return _recognizer.RecognizeOnceAsync().Result;
        }

        public void Speak(string text)
        {
            _synthesizer.Speak(text);
        }

        public bool HasSpeech(SpeechRecognitionResult result)
        {
            return result.Reason == ResultReason.RecognizedSpeech;
        }

        public bool HasNoMatch(SpeechRecognitionResult result)
        {
            return result.Reason == ResultReason.NoMatch;
        }

        public bool HasCancelledDueToEndOfStream(SpeechRecognitionResult result)
        {
            return result.Reason == ResultReason.Canceled && CancellationDetails.FromResult(result).Reason == CancellationReason.EndOfStream;
        }

        public bool HasCancelledWithErrors(SpeechRecognitionResult result)
        {
            return result.Reason == ResultReason.Canceled && CancellationDetails.FromResult(result).Reason == CancellationReason.Error;
        }

        public string GetRawText(SpeechRecognitionResult result)
        {
            return result.Text;
        }

        public string GetText(SpeechRecognitionResult result)
        {
            return result.Text.TrimEnd('.').TrimEnd('!').TrimEnd('?');
        }

        public int? GetInt(SpeechRecognitionResult result)
        {
            int i = 0;
            if (Int32.TryParse(result.Text.TrimEnd('.'),out i))
            {
                return i;
            }
            else
            {
                return null;
            }

            /*
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(result.Text);
            if (match.Success)
            {
                return Int32.Parse(match.Value);
            }
            else
            {
                return null;
            }
            */
        }

        public double? GetDouble(SpeechRecognitionResult result)
        {
            double d = 0;
            if(Double.TryParse(result.Text.TrimEnd('.'),out d))
            {
                return d;
            }
            else
            {
                return null;
            }
        }

    }
}
