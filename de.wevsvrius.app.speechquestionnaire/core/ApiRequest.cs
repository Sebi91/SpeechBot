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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace de.wevsvrius.app.speechquestionnaire.core
{
    public class ApiRequest
    {
        private HttpRequestMessage _requestMessage = new HttpRequestMessage();

        /// <summary>
        /// Gets or sets the nested <see cref="HttpRequestMessage"/>
        /// </summary>
        internal HttpRequestMessage RequestMessage
        {
            get
            {
                return _requestMessage;
            }

            set
            {
                _requestMessage = value;
            }
        }

        private ApiRequest()
        {

        }

        public override string ToString()
        {
            return _requestMessage.Method.ToString() + " " + _requestMessage.RequestUri.ToString();
        }


        /// <summary>
        /// Creates a minimal <see cref="ApiRequest"/> having the passed method and url.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ApiRequest Create(System.Net.Http.HttpMethod method, string url)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(url);
            requestMessage.Method = method;

            ApiRequest apiRequest = new ApiRequest();
            apiRequest.RequestMessage = requestMessage;
            return apiRequest;
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> having the passed method, url, payload and content-type header.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ApiRequest Create(System.Net.Http.HttpMethod method, string url, string payload, string contentType)
        {
            return ApiRequest.Create(method, url, new StringContent(payload), contentType);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> having the passed method, url, payload and content-type header.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ApiRequest Create(System.Net.Http.HttpMethod method, string url, byte[] payload, string contentType)
        {
            return ApiRequest.Create(method, url, new ByteArrayContent(payload), contentType);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> having the passed method, url, payload and content-type header.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ApiRequest Create(System.Net.Http.HttpMethod method, string url, HttpContent payload, string contentType)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(url);
            requestMessage.Method = method;

            requestMessage.Content = payload;
            //try to split content-type
            string[] split = contentType.Split(';');
            requestMessage.Content.Headers.ContentType.MediaType = split[0];

            for (int i = 1; i < split.Length; i++)
            {
                string[] parameter = split[i].Trim().Split('=');
                if (parameter.Length == 1 && !String.IsNullOrWhiteSpace(parameter[0]))
                {
                    requestMessage.Content.Headers.ContentType.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue(parameter[0].Trim()));
                }
                else if (parameter.Length == 2 && !String.IsNullOrWhiteSpace(parameter[0]) && !String.IsNullOrWhiteSpace(parameter[1]))
                {
                    if (parameter[0].CompareTo("charset") == 0)
                    {
                        requestMessage.Content.Headers.ContentType.CharSet = parameter[1].Trim();
                    }
                    else
                    {
                        requestMessage.Content.Headers.ContentType.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue(parameter[0].Trim(), parameter[1].Trim()));
                    }
                }
            }

            ApiRequest apiRequest = new ApiRequest();
            apiRequest.RequestMessage = requestMessage;
            return apiRequest;
        }
        /// <summary>
        /// Creates an <see cref="ApiRequest"/> based on the passed <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ApiRequest Create(HttpRequestMessage request)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.RequestMessage = request;
            return apiRequest;
        }
    }
}
