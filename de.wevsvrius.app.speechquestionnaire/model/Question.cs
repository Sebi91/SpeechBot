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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.wevsvrius.app.speechquestionnaire.model
{
    public class Question
    {
        private IList<Option> _options = new List<Option>();

        /// <summary>
        /// The ID of the User this Question is associated with
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The ID of the Questionnaire this Question is associated with
        /// </summary>
        [JsonProperty("questionnaireId")]
        public string QuestionnaireId { get; set; }

        /// <summary>
        /// The ID of the Question
        /// </summary>
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        /// <summary>
        /// The displayed question, e.g. "Wie alt sind Sie?"
        /// </summary>
        [JsonProperty("question")]
        public string QuestionText { get; set; }

        /// <summary>
        /// Further details, e.g. "Bitte geben Sie Ihr Alter an"
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The answer, e.g. "40"
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// The data type that is expected
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The category of this Question
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// The set of Options
        /// </summary>
        [JsonProperty("Options")]
        public IList<Option> OptionsList
        {
            get
            {
                return _options;
            }

            set
            {
                _options = value;
            }
        }

        [JsonProperty("_links")]
        public Links Links { get; set; }
    }
}
