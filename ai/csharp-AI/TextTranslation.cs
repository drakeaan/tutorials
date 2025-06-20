﻿using Azure;
using Azure.AI.Translation.Text;
using Azure.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAI;

internal class TextTranslation
{
    public async Task Run()
    {
        // Setup a listener to monitor logged events.
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

        AzureKeyCredential credential = new("<apiKey>");
        TextTranslationClient client = new(credential, "<region>");

        // Get Supported Languages
        try
        {
            Response<GetLanguagesResult> response = await client.GetLanguagesAsync(cancellationToken: CancellationToken.None).ConfigureAwait(false);
            GetLanguagesResult languages = response.Value;

            Console.WriteLine($"Number of supported languages for translate operations: {languages.Translation.Count}.");
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }


        // Translate
        try
        {
            string targetLanguage = "cs";
            string inputText = "This is a test.";

            Response<IReadOnlyList<TranslatedTextItem>> response = await client.TranslateAsync(targetLanguage, inputText).ConfigureAwait(false);
            IReadOnlyList<TranslatedTextItem> translations = response.Value;
            TranslatedTextItem translation = translations.FirstOrDefault();

            Console.WriteLine($"Detected languages of the input text: {translation?.DetectedLanguage?.Language} with score: {translation?.DetectedLanguage?.Score}.");
            Console.WriteLine($"Text was translated to: '{translation?.Translations?.FirstOrDefault().To}' and the result is: '{translation?.Translations?.FirstOrDefault()?.Text}'.");
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }

        // Transliterate
        try
        {
            string language = "zh-Hans";
            string fromScript = "Hans";
            string toScript = "Latn";

            string inputText = "这是个测试。";

            Response<IReadOnlyList<TransliteratedText>> response = await client.TransliterateAsync(language, fromScript, toScript, inputText).ConfigureAwait(false);
            IReadOnlyList<TransliteratedText> transliterations = response.Value;
            TransliteratedText transliteration = transliterations.FirstOrDefault();

            Console.WriteLine($"Input text was transliterated to '{transliteration?.Script}' script. Transliterated text: '{transliteration?.Text}'.");
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }


        // Break sentence
        try
        {
            string inputText = "How are you? I am fine. What did you do today?";

            Response<IReadOnlyList<BreakSentenceItem>> response = await client.FindSentenceBoundariesAsync(inputText).ConfigureAwait(false);
            IReadOnlyList<BreakSentenceItem> brokenSentences = response.Value;
            BreakSentenceItem brokenSentence = brokenSentences.FirstOrDefault();

            Console.WriteLine($"Detected languages of the input text: {brokenSentence?.DetectedLanguage?.Language} with score: {brokenSentence?.DetectedLanguage?.Score}.");
            Console.WriteLine($"The detected sentece boundaries: '{string.Join(",", brokenSentence?.SentLen)}'.");

        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }


        // Dictionary Lookup
        try
        {
            string sourceLanguage = "en";
            string targetLanguage = "es";
            string inputText = "fly";

            Response<IReadOnlyList<DictionaryLookupItem>> response = await client.LookupDictionaryEntriesAsync(sourceLanguage, targetLanguage, inputText).ConfigureAwait(false);
            IReadOnlyList<DictionaryLookupItem> dictionaryEntries = response.Value;
            DictionaryLookupItem dictionaryEntry = dictionaryEntries.FirstOrDefault();

            Console.WriteLine($"For the given input {dictionaryEntry?.Translations?.Count} entries were found in the dictionary.");
            Console.WriteLine($"First entry: '{dictionaryEntry?.Translations?.FirstOrDefault()?.DisplayTarget}', confidence: {dictionaryEntry?.Translations?.FirstOrDefault()?.Confidence}.");

        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }


        // Dictionary Examples
        try
        {
            string sourceLanguage = "en";
            string targetLanguage = "es";
            IEnumerable<InputTextWithTranslation> inputTextElements = new[]
            {
                    new InputTextWithTranslation("fly", "volar")
                };

            Response<IReadOnlyList<DictionaryExampleItem>> response = await client.LookupDictionaryExamplesAsync(sourceLanguage, targetLanguage, inputTextElements).ConfigureAwait(false);
            IReadOnlyList<DictionaryExampleItem> dictionaryEntries = response.Value;
            DictionaryExampleItem dictionaryEntry = dictionaryEntries.FirstOrDefault();

            Console.WriteLine($"For the given input {dictionaryEntry?.Examples?.Count} examples were found in the dictionary.");
            DictionaryExample firstExample = dictionaryEntry?.Examples?.FirstOrDefault();
            Console.WriteLine($"Example: '{string.Concat(firstExample.TargetPrefix, firstExample.TargetTerm, firstExample.TargetSuffix)}'.");

        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }


        // Troubleshooting
        try
        {
            var translation = client.TranslateAsync(Array.Empty<string>(), new[] { new InputText { Text = "This is a Test" } }).ConfigureAwait(false);
        }
        catch (RequestFailedException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}