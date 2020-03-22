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
    public class ApiResponse
    {
        private HttpResponseMessage _responseMessage = new HttpResponseMessage();

        public int StatusCode
        {
            get
            {
                return (int)_responseMessage.StatusCode;
            }
        }

        public HttpResponseMessage ResponseMessage
        {
            get
            {
                return _responseMessage;
            }

            internal set
            {
                _responseMessage = value;
            }
        }

        internal ApiResponse(HttpResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
        }

       
        public override string ToString()
        {
            return ((int)_responseMessage.StatusCode) + " " + _responseMessage.StatusCode.ToString();
        }
    }
}
