# SpeechBot
#wirvsvirus

Für unseren Prototypen existiert vorerst nur ein Kanal zur Nutzung unseres OpenHealthCheck Services. Die Speech Bot Applikation ermöglicht die Benutzung von OpenHealthCheck mittels Sprache und soll einen Telefonservice repräsentieren. Der Anwender wird durch ein Sprachdialog (Spracherkennung und Sprachausgabe) durch den Fragebogen geführt. Speech Bot kommuniziert dabei im Hintergrund mit dem zentralen Backend und holt den aktuellen Fragebogen Frage für Frage über die RESTful API ab. Nach der Beantwortung einer Frage (das Ergebnis wird per HTTP PUT Request an das Backend übermittelt) wird per HTTP GET Request die nächste Frage geladen, wobei die Reihenfolge durch das Backend festgelegt ist und sich dynamisch, basierend auf der letzten Antwort ändern kann.

## Getting Started
- A valid subscription for Microsoft Azure Speech services is required in order to use this application
- Replace "KEY" and "REGION" in SpeechRecognizer.css with your subscription key and the region that is associated to your subscription (e.g. "westus")
- Make sure that either x86 or x64 is chosen as the active solution platform (go to Build > Configuration Manager, further instructions can be found here: https://docs.microsoft.com/de-de/azure/cognitive-services/speech-service/quickstarts/setup-platform?tabs=dotnet%2Cwindows%2Cjre&pivots=programming-language-csharp)
- Add the following packages via NuGet Package Manager: Newtonsoft.Json and Microsoft.CognitiveServices.Speech.csharp

## Dependencies
- Newtonsoft.json
- Microsoft.CognitiveServices.Speech.csharp (a valid Microsoft Azure Speech services subscription is required)
