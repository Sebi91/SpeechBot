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
using de.wevsvrius.app.speechquestionnaire.model;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.wevsvrius.app.speechquestionnaire.core
{
    public class Handler
    {
        public const string NO_RESULT_RESPONSE = "Das habe ich leider nicht verstanden. Bitte wiederholen Sie Ihre Antwort!";
        public const string ERROR_RESPONSE = "Es trat ein Fehler auf. Wir versuchen es erneut. Bitte wiederholen Sie Ihre Antwort!";
        public const string ERROR_INVALID_ANSWER = "Ihre Eingabe ist leider nicht korrekt. Bitte versuchen Sie es erneut!";

        public string[] thanks = new[] { "Vielen Dank", "Dankeschön", "Perfekt, vielen Dank", "Besten Dank" };

        public void Run()
        {
            //Users users = Api.Instance.QueryUserList();

            //step 1: start with questionnaire
            Speak("Hallo und Herzlich Willkommen. Wir werden Ihnen nun einige Fragen stellen.");

            //step 2: determine user ID
            Speak("Bitte nennen Sie mir Ihren Code.");
            User user = FindUserIdMatchingSocialIdOrReturnDefault(GetSpeechAsText(), Api.Instance.QueryUserList());
            Questionnaire questionnaire = Api.Instance.CreateAndLoadNewQuestionaire(user.UserId);
            SpeakThanks();
            //Speak("Hallo " + user.Name + " " + user.Surname);

            string nextQuestion = ProcessQuestion(questionnaire.Links.First.Href);
            while(!String.IsNullOrWhiteSpace(nextQuestion) && !nextQuestion.Contains("summary"))
            {
                nextQuestion = ProcessQuestion(nextQuestion);
            }
            if(nextQuestion == null)
            {
                Console.WriteLine("ERROR: next question is null");
            }
            else
            {
                ProcessSummary(nextQuestion);
            }
        }

        private string ProcessQuestion(string uri)
        {
            Question question = Api.Instance.QueryQuestion(uri);
            if(question == null)
            {
                Console.WriteLine("API CLIENT ERROR: Cannot query " + uri);
                return null;
            }
            else
            {
                //speak question
                if (!String.IsNullOrWhiteSpace(question.QuestionText))
                {
                    Speak(question.QuestionText);
                }

                //speak description
                if (!String.IsNullOrWhiteSpace(question.Description))
                {
                    Speak(question.Description);
                }

                //speak options
                if (question.OptionsList.Count > 1)
                {
                    
                    Speak("Sie haben folgende Antwortmöglichkeiten:");
                    foreach(Option option in question.OptionsList)
                    {
                        Speak(option.Name);
                    }
                }

                //answer:
                string nextQuestion = ProcessAnswer(uri,question);
                while (String.IsNullOrWhiteSpace(nextQuestion))
                {
                    Speak(ERROR_INVALID_ANSWER);
                    nextQuestion = ProcessAnswer(uri, question);
                }
                SpeakThanks();
                return nextQuestion;
            }
        }

        private string ProcessAnswer(string uri, Question question)
        {
            //answer:
            string val = "";
            if (question.Type.CompareTo("int") == 0)
            {
                int i = GetSpeechAsInt();
                val = "" + i;
            }
            else if (question.Type.CompareTo("double") == 0)
            {
                double d = GetSpeechAsDouble();
                val = "" + d;
                val = val.Replace(",", ".");
            }
            else //atomic and string
            {
                string text = GetSpeechAsText();
                val = text;
            }

            Question updatedQuestion = Api.Instance.UpdateQuestion(uri, val.ToLower());
            if(updatedQuestion == null)
            {
                Console.WriteLine("Question " + question.QuestionId + " cannot be updated. The answer was " + val);
                return null;
            }
            else
            {
                return updatedQuestion.Links.Next.Href;
            }
        }

        private void ProcessSummary(string uri)
        {
            Summary summary = Api.Instance.QuerySummary(uri);
            if(summary == null)
            {
                Console.WriteLine("ERROR: Cannot query summary with URI: " + uri);
            }
            else
            {
                Speak("Sie sind zu " + summary.DegreeOfSickness + " Prozent mit Covid-19 infiziert");
                Speak("Vielen Dank für Ihren Anruf. Melden Sie sich bald wieder. Wir wünschen eine gute Besserung");
            }
        }

        public int GetSpeechAsInt()
        {
            SpeechRecognitionResult result = Speech.Instance.Recognize();
            if (Speech.Instance.HasSpeech(result))
            {
                int? i = Speech.Instance.GetInt(result);
                if (i.HasValue)
                {
                    Console.WriteLine("Recognized: " + i);
                    return i.Value;
                }
                else
                {
                    Console.WriteLine("Recognized: " + result.Text);
                    Speech.Instance.Speak(NO_RESULT_RESPONSE);
                    return GetSpeechAsInt();
                }
            }
            else if(Speech.Instance.HasNoMatch(result))
            {
                Console.WriteLine("<No match>");
                Speech.Instance.Speak(NO_RESULT_RESPONSE);
                return GetSpeechAsInt();
            }
            else if (Speech.Instance.HasCancelledDueToEndOfStream(result))
            {
                Console.WriteLine("Error: End of Stream");
                Speech.Instance.Speak(ERROR_RESPONSE);
                return GetSpeechAsInt();
            }
            else if (Speech.Instance.HasCancelledWithErrors(result))
            {
                Console.WriteLine("Error Code: " + CancellationDetails.FromResult(result).ErrorCode + "; Details: " + CancellationDetails.FromResult(result).ErrorDetails);
                Speech.Instance.Speak(ERROR_RESPONSE);
                return GetSpeechAsInt();
            }
            else
            {
                Console.WriteLine("Result: " + result.Reason);
                Speech.Instance.Speak(NO_RESULT_RESPONSE);
                return GetSpeechAsInt();
            }
        }

        public double GetSpeechAsDouble()
        {
            SpeechRecognitionResult result = Speech.Instance.Recognize();
            if (Speech.Instance.HasSpeech(result))
            {
                double? d = Speech.Instance.GetDouble(result);
                if (d.HasValue)
                {
                    Console.WriteLine("Recognized: " + d);
                    return d.Value;
                }
                else
                {
                    Console.WriteLine("Recognized: " + result.Text);
                    Speech.Instance.Speak(NO_RESULT_RESPONSE);
                    return GetSpeechAsDouble();
                }
            }
            else if (Speech.Instance.HasNoMatch(result))
            {
                Console.WriteLine("<No match>");
                Speech.Instance.Speak(NO_RESULT_RESPONSE);
                return GetSpeechAsDouble();
            }
            else if (Speech.Instance.HasCancelledDueToEndOfStream(result))
            {
                Console.WriteLine("Error: End of Stream");
                Speech.Instance.Speak(ERROR_RESPONSE);
                return GetSpeechAsDouble();
            }
            else if (Speech.Instance.HasCancelledWithErrors(result))
            {
                Console.WriteLine("Error Code: " + CancellationDetails.FromResult(result).ErrorCode + "; Details: " + CancellationDetails.FromResult(result).ErrorDetails);
                Speech.Instance.Speak(ERROR_RESPONSE);
                return GetSpeechAsDouble();
            }
            else
            {
                Console.WriteLine("Result: " + result.Reason);
                Speech.Instance.Speak(NO_RESULT_RESPONSE);
                return GetSpeechAsDouble();
            }
        }

        private string GetSpeechAsText()
        {
            SpeechRecognitionResult result = Speech.Instance.Recognize();
            if (Speech.Instance.HasSpeech(result))
            {
                Console.WriteLine("Recognized: " + result.Text);
                return Speech.Instance.GetText(result);
            }
            else if (Speech.Instance.HasNoMatch(result))
            {
                Console.WriteLine("<No match>");
                Speech.Instance.Speak(NO_RESULT_RESPONSE);
                return GetSpeechAsText();
            }
            else if (Speech.Instance.HasCancelledDueToEndOfStream(result))
            {
                Console.WriteLine("Error: End of Stream");
                Speech.Instance.Speak(ERROR_RESPONSE);
                return GetSpeechAsText();
            }
            else if (Speech.Instance.HasCancelledWithErrors(result))
            {
                Console.WriteLine("Error Code: " + CancellationDetails.FromResult(result).ErrorCode + "; Details: " + CancellationDetails.FromResult(result).ErrorDetails);
                Speech.Instance.Speak(ERROR_RESPONSE);
                return GetSpeechAsText();
            }
            else
            {
                Console.WriteLine("Result: " + result.Reason);
                Speech.Instance.Speak(NO_RESULT_RESPONSE);
                return GetSpeechAsText();
            }
        }

        private void Speak(string text)
        {
            Console.WriteLine("System: " + text);
            Speech.Instance.Speak(text);
        }

        private void SpeakThanks()
        {
            string t = this.thanks[(new Random().Next() % this.thanks.Length)];
            Speak(t);
        }

        private User FindUserIdMatchingSocialIdOrReturnDefault(string socialId,Users users)
        {
            foreach(User user in users.UserList)
            {
                if (user.SocialId.ToLower().CompareTo(socialId.ToLower()) == 0)
                {
                    return user;
                }
            }
            if(users.UserList.Count > 0)
            {
                return users.UserList[0];
            }
            else
            {
                return null;
            }
        }


    }
}
