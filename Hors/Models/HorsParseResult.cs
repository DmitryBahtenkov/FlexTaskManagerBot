using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hors.Utils;

namespace Hors.Models
{
    public class HorsParseResult
    {
        public string SourceText { get; }
        public List<string> Tokens { get; }
        public string Text { get; }
        public List<DateTimeToken> Dates { get; }
        
        private string _textWithTokens;
        private readonly List<DateTimeToken> _fullDates;
        private readonly HashSet<string> _tokensToRemove = new HashSet<string>();

        public HorsParseResult(string sourceText, List<string> tokens, List<DateTimeToken> dates)
        {
            SourceText = sourceText;
            _fullDates = dates;
            Dates = CreateDates(dates);
            Tokens = tokens.Where(t => !_tokensToRemove.Contains(t)).ToList();
            Text = Helpers.TrimPunctuation(CreateText(false)).Trim();
        }

        private List<DateTimeToken> CreateDates(List<DateTimeToken> dates)
        {
            var duplicateSeen = new HashSet<double>();
            var datesOut = new List<DateTimeToken>();

            for (var i = 0; i < dates.Count; i++)
            {
                var date = dates[i];
                if (date.GetDuplicateGroup() == -1)
                {
                    datesOut.Add(date);
                }
                else if (!duplicateSeen.Contains(date.GetDuplicateGroup()))
                {
                    duplicateSeen.Add(date.GetDuplicateGroup());
                    datesOut.Add(date);
                }
                else
                {
                    _tokensToRemove.Add($"{{{i}}}");
                }
            }

            return datesOut;
        }

        private string CreateText(bool insertTokens)
        {
            var text = SourceText;
            var skippedDates = new HashSet<DateTimeToken>();
            
            // loop dates from last to first
            for (var i = _fullDates.Count - 1; i >= 0; i--)
            {
                var date = _fullDates[i];
                if (skippedDates.Contains(date)) continue;
                
                var sameDates = _fullDates.Where(d => d.StartIndex == date.StartIndex && !skippedDates.Contains(d)).ToList();
                var tokensToInsert = new List<string>();
                
                foreach (var oDate in sameDates)
                {
                    skippedDates.Add(oDate);
                    var indexInList = _fullDates.IndexOf(oDate);
                    tokensToInsert.Add($"{{{indexInList}}}");
                }

                text = text.Substring(0, date.StartIndex)
                       + (insertTokens && Dates.Contains(date) ? string.Join(" ", tokensToInsert) : "")
                       + (date.EndIndex < text.Length ? text.Substring(date.EndIndex) : "");
            }

            return Regex.Replace(text.Trim(), @"\s{2,}", " ");
        }
        public string CreateText(int index)
        {
            var text = SourceText;
            var skippedDates = new HashSet<DateTimeToken>();
            
            var mainDate = _fullDates[index];
                
            skippedDates.Add(mainDate);

            var startLength = text.Length;

            text = text.Substring(0, mainDate.StartIndex)
                   + (mainDate.EndIndex < text.Length ? text.Substring(mainDate.EndIndex) : "");

            var offset = startLength - text.Length;
            
            // loop dates from last to first
            for (var i = _fullDates.Count - 1; i >= 0; i--)
            {
                var date = _fullDates[i];
                if (skippedDates.Contains(date))
                {
                    continue;
                }
                
                var sameDates = _fullDates.Where(d => d.StartIndex == date.StartIndex && !skippedDates.Contains(d)).ToList();
                var tokensToInsert = new List<(int Index, string Str)>();
                
                foreach (var oDate in sameDates)
                {
                    skippedDates.Add(oDate);
                    var indexInList = _fullDates.IndexOf(oDate);
                    var token = $"{{{indexInList}}}";
                    tokensToInsert.Add((oDate.StartIndex, token));
                }

                
                int endIndex;
                if (date.EndIndex > mainDate.EndIndex)
                {
                    endIndex = date.EndIndex - offset;
                }
                else
                {
                    endIndex = date.EndIndex;
                    offset = 0;
                }
                
                text = text.Substring(0, date.StartIndex - offset)
                       + (Dates.Contains(date) ? string.Join(" ", tokensToInsert.Select(x => x.Str)) : "")
                       + (endIndex < text.Length ? text.Substring(date.EndIndex - offset) : "");

                foreach (var token in tokensToInsert)
                {
                    var parts = text.Split(token.Str);
                    var right = parts[1];
                    var endIndexForReplacement = text.IndexOf(right, StringComparison.Ordinal) + offset;
                    var textForReplacement = SourceText.Substring(token.Index, endIndexForReplacement - token.Index).Trim();
                    text = text.Replace(token.Str, textForReplacement);
                }
            }

            return Regex.Replace(text.Trim(), @"\s{2,}", " ");
        }
        

        public string CleanTextWithTokens => string.Join(" ", Tokens);

        public string TextWithTokens
        {
            get
            {
                if (string.IsNullOrEmpty(_textWithTokens))
                {
                    _textWithTokens = CreateText(true);
                }

                return _textWithTokens;
            }
        }

        public override string ToString()
        {
            return $"{CleanTextWithTokens} | {string.Join("; ", Dates.Select(d => d.ToString()))}";
        }
    }
}