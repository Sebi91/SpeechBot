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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace de.wevsvrius.app.speechquestionnaire.core
{
    public class Api
    {
        public const string HOST = "HOST";
        public const string USERS_ENDPOINT = HOST+@"/api/v1/users";
        public const string QUESTIONNAIRES_ENDPOINT = HOST + @"/api/v1/users/{userId}/questionnaires";
        public const string QUESTIONNAIRE_ENDPOINT = HOST + @"/api/v1/users/{userId}/questionnaires/{questionnaireId}";

        private static Api _instance;
        private ApiClient _client;

        public static Api Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Api();
                }
                return _instance;
            }
        }

        private Api()
        {
            _client = new ApiClient();
        }

        public Users QueryUserList()
        {
            ApiResponse response = _client.Send(ApiRequest.Create(HttpMethod.Get, USERS_ENDPOINT));
            if(response.StatusCode == 200)
            {
                string json = response.ResponseMessage.Content.ReadAsStringAsync().Result;
                return JsonSerializer.DeserializeJson<Users>(json);
            }
            else
            {
                return null;
            }
        }

        public Questionnaire CreateAndLoadNewQuestionaire(string userId)
        {
            ApiResponse response = _client.Send(ApiRequest.Create(HttpMethod.Post, QUESTIONNAIRES_ENDPOINT.Replace("{userId}", userId)));
            if(response.StatusCode == 201)
            {

                response = _client.Send(ApiRequest.Create(HttpMethod.Get, HOST+response.ResponseMessage.Headers.Location.AbsolutePath));
                if(response.StatusCode == 200)
                {
                    string json = response.ResponseMessage.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.DeserializeJson<Questionnaire>(json);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Question QueryQuestion(string uri)
        {
            ApiResponse response = _client.Send(ApiRequest.Create(HttpMethod.Get, uri));
            if(response.StatusCode == 200)
            {
                string json = response.ResponseMessage.Content.ReadAsStringAsync().Result;
                return JsonSerializer.DeserializeJson<Question>(json);
            }
            else
            {
                return null;
            }
        }

        public Question UpdateQuestion(string uri, string value)
        {
            QuestionAnswer answer = new QuestionAnswer() { Value = value };
            string payload = JsonSerializer.SerializeJson(answer);

            ApiResponse response = _client.Send(ApiRequest.Create(HttpMethod.Put, uri, payload, "application/json"));
            if(response.StatusCode == 200)
            {
                string json = response.ResponseMessage.Content.ReadAsStringAsync().Result;
                return JsonSerializer.DeserializeJson<Question>(json);
            }
            else
            {
                return null;
            }
        }

        public Summary QuerySummary(string uri)
        {
            ApiResponse response = _client.Send(ApiRequest.Create(HttpMethod.Get, uri));
            if(response.StatusCode == 200)
            {
                string json = response.ResponseMessage.Content.ReadAsStringAsync().Result;
                return JsonSerializer.DeserializeJson<Summary>(json);
            }
            else
            {
                return null;
            }
        }
    }
}
