using System;
using Sensory.Api.V1.Audio;
using System.Linq;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// FullTranscriptAggregator aggregates and stores transcription responses.
    /// This class can be used to maintain the full transcript returned from the server.
    /// </summary>
    public class FullTranscriptAggregator
    {
        /// <summary>
        /// Hold onto the entire transcript as an array of words
        /// </summary>
        private TranscribeWord[] CurrentWordList = new TranscribeWord[0];

        /// <summary>
        /// Process a single, sliding-window response from the server
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void ProcessResponse(TranscribeWordResponse response)
        {
            // If nothing is returned, do nothing
            if (response == null || response.Words.Count == 0)
            {
                return;
            }

            // Get the expected transcript size from the index of the last word.
            ulong responseSize = response.LastWordIndex + 1;

            // Grow the word buffer if the incoming transcript is larger.
            if (responseSize > Convert.ToUInt64(this.CurrentWordList.Length))
            {
                // Resize array
                Array.Resize(ref this.CurrentWordList, Convert.ToInt32(responseSize));
            }

            // Loop through returned words and set the returned value at the specified index in currentWordList
            foreach (TranscribeWord word in response.Words)
            {
                this.CurrentWordList[word.WordIndex] = word;
            }

            // Check if the transcript is smaller than our currentWordList
            if (responseSize < Convert.ToUInt64(this.CurrentWordList.Length))
            {
                // Resize array
                Array.Resize(ref this.CurrentWordList, Convert.ToInt32(responseSize));
            }
        }

        /// <summary>
        /// Returns the cache of words returned from the server
        /// </summary>
        public TranscribeWord[] GetCurrentWordlist() {
            return this.CurrentWordList;
        }

        /// <summary>
        /// Returns the full transcript as computed from the current word list
        /// </summary>
        /// <param name=""></param>
        public string GetCurrentTranscript(string delimiter=" ") {
            if (this.CurrentWordList.Length == 0) {
                return "";
            }

            return string.Join(delimiter, this.CurrentWordList.Select(word => word.Word.Trim()));
        }
    }
}
