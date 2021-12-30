using System.Collections.Generic;

namespace Scheduler
{
    public class TextManager
    {
        private Dictionary<string, string> translations;
        private string culture;

        public TextManager(string culture)
        {
            translations = new Dictionary<string, string>();
            this.culture = culture;
            switch (culture)
            {
                case "EN-US":
                    AddEnUsTexts();
                    break;
                case "EN-UK":
                    AddEnUkTexts();
                    break;
                case "ES-ES":
                    AddEsEsTexts();
                    break;
                default:
                    AddEnUkTexts();
                    break;
            }
        }

        public string GetDatetimeFormat()
        {
            switch (culture)
            {
                case "EN-US":
                    return "MM/dd/yyyy";
                case "EN-UK":
                    return "dd/MM/yyyy";
                case "ES-ES":
                    return "dd/MM/yyyy";
                default:
                    return "dd/MM/yyyy";
            }
        }

        public string GetText(string textKey)
        {
            if (translations.ContainsKey(textKey))
            {
                return translations[textKey];
            }
            return string.Empty;
        }

        private void AddEnUsTexts()
        {
            translations.Add("ONCE_OCCURRENCE", "Occurs once. Schedule will be used on {0} at {1}");
            translations.Add("DATE_OVER_END", "Will not occur. Schedule will end on {0}");
            translations.Add("NO_VALID_FREQUENCY", "No valid frequency was specified");
            translations.Add("STARTING_ON", "starting on");
            translations.Add("OCCURS_EVERYDAY", "Occurs everyday");
            translations.Add("NO_DAY_TYPE", "No day type was specified");
            translations.Add("NO_FREQUENCY", "No frequency of execution was specified");
            translations.Add("INVALID_INTERVAL", "The specified monthly interval is invalid");
            translations.Add("INVALID_DAY", "The specified day of execution is invalid");
            translations.Add("NO_MONTHLY_FREQUENCY", "No monthly frequency was specified");
            translations.Add("NO_VALID_WEEKDAY", "No valid day of the week was indicated");
            translations.Add("INCORRECT_WEEKLY_FREQUENCY", "Incorrect weekly frequency");
            translations.Add("NO_WEEKLY_FREQUENCY", "No weekly frequency was specified");
            translations.Add("NO_DAILY_FREQUENCY", "No daily frequency was specified");
            translations.Add("END_DATE_AFTER_START", "The end date must come after the start date");
            translations.Add("INVALID_ENDDATE", "The specified end date is not valid");
            translations.Add("INVALID_STARTDATE", "The specified start date is not valid");
            translations.Add("DAILY_OCCURRENCE", "Occurs the {0} of every {1} {2} ");
            translations.Add("RECURRING_OCCURENCE", "Occurs the {0} {1} of every {2} {3} ");
            translations.Add("OCCURRENCE_STR", "Occurs every {0} weeks on {1} ");
            translations.Add("OCCURRENCE_STR_RECURRING", "every {0} {1} between {2} and {3}");
            translations.Add("OCCURRENCE_STR_ONCE", "once at {0}");
            translations.Add("SECONDS", "seconds");
            translations.Add("HOURS", "hours");
            translations.Add("MINUTES", "minutes");
            translations.Add("MONTHS", "months");
            translations.Add("MONTH", "month");
            translations.Add("WEEKS", "weeks");
            translations.Add("WEEK", "week");
            translations.Add("ADDITION", "and");
        }

        private void AddEnUkTexts()
        {
            translations.Add("ONCE_OCCURRENCE", "Occurs once. Schedule will be used on {0} at {1}");
            translations.Add("DATE_OVER_END", "Will not occur. Schedule will end on {0}");
            translations.Add("NO_VALID_FREQUENCY", "No valid frequency was specified");
            translations.Add("STARTING_ON", "starting on");
            translations.Add("OCCURS_EVERYDAY", "Occurs everyday");
            translations.Add("NO_DAY_TYPE", "No day type was specified");
            translations.Add("NO_FREQUENCY", "No frequency of execution was specified");
            translations.Add("INVALID_INTERVAL", "The specified monthly interval is invalid");
            translations.Add("INVALID_DAY", "The specified day of execution is invalid");
            translations.Add("NO_MONTHLY_FREQUENCY", "No monthly frequency was specified");
            translations.Add("NO_VALID_WEEKDAY", "No valid day of the week was indicated");
            translations.Add("INCORRECT_WEEKLY_FREQUENCY", "Incorrect weekly frequency");
            translations.Add("NO_WEEKLY_FREQUENCY", "No weekly frequency was specified");
            translations.Add("NO_DAILY_FREQUENCY", "No daily frequency was specified");
            translations.Add("END_DATE_AFTER_START", "The end date must come after the start date");
            translations.Add("INVALID_ENDDATE", "The specified end date is not valid");
            translations.Add("INVALID_STARTDATE", "The specified start date is not valid");
            translations.Add("DAILY_OCCURRENCE", "Occurs the {0} of every {1} {2} ");
            translations.Add("RECURRING_OCCURENCE", "Occurs the {0} {1} of every {2} {3} ");
            translations.Add("OCCURRENCE_STR", "Occurs every {0} weeks on {1} ");
            translations.Add("OCCURRENCE_STR_RECURRING", "every {0} {1} between {2} and {3}");
            translations.Add("OCCURRENCE_STR_ONCE", "once at {0}");
            translations.Add("SECONDS", "seconds");
            translations.Add("HOURS", "hours");
            translations.Add("MINUTES", "minutes");
            translations.Add("MONTHS", "months");
            translations.Add("MONTH", "month");
            translations.Add("WEEKS", "weeks");
            translations.Add("WEEK", "week");
            translations.Add("ADDITION", "and");
        }

        private void AddEsEsTexts()
        {
            translations.Add("ONCE_OCCURRENCE", "Ocurre una vez. La programacion se usará el {0} a las {1}");
            translations.Add("DATE_OVER_END", "No ocurrira. La programacion acabará el {0}");
            translations.Add("NO_VALID_FREQUENCY", "No valid frequency was specified");
            translations.Add("STARTING_ON", "empezando el");
            translations.Add("OCCURS_EVERYDAY", "Ocurre todos los dias");
            translations.Add("NO_DAY_TYPE", "no se ha especificado tipo de dia");
            translations.Add("NO_FREQUENCY", "No se ha especificado frecuencia de ejecucion");
            translations.Add("INVALID_INTERVAL", "El intervalo mensual especificado no es válido");
            translations.Add("INVALID_DAY", "El dia de ejecución especificado no es válido");
            translations.Add("NO_MONTHLY_FREQUENCY", "No se ha especificado frecuencia mensual");
            translations.Add("NO_VALID_WEEKDAY", "No se ha indicado un dia de la semana válido");
            translations.Add("INCORRECT_WEEKLY_FREQUENCY", "Frecuencia semanal incorrecta");
            translations.Add("NO_WEEKLY_FREQUENCY", "No se ha especificado frecuencia semanal");
            translations.Add("NO_DAILY_FREQUENCY", "No se ha especificado frecuencia diaria");
            translations.Add("END_DATE_AFTER_START", "La fecha de fin debe ser posterior a la fecha de inicio");
            translations.Add("INVALID_ENDDATE", "La fecha de fin especificada no es válida");
            translations.Add("INVALID_STARTDATE", "La fecha de inicio especificada no es válida");
            translations.Add("DAILY_OCCURRENCE", "Ocurre el {0} de cada {1} {2} ");
            translations.Add("RECURRING_OCCURENCE", "Ocurre el {0} {1} de cada {2} {3} ");
            translations.Add("OCCURRENCE_STR", "Ocurre cada {0} semanas el {1} ");
            translations.Add("OCCURRENCE_STR_RECURRING", "cada {0} {1} entre las {2} y las {3}");
            translations.Add("OCCURRENCE_STR_ONCE", "una vez a las {0}");
            translations.Add("SECONDS", "segundos");
            translations.Add("HOURS", "horas");
            translations.Add("MINUTES", "minutos");
            translations.Add("MONTHS", "meses");
            translations.Add("MONTH", "mes");
            translations.Add("WEEKS", "semanas");
            translations.Add("WEEK", "semana");
            translations.Add("ADDITION", "y");
        }
    }
}
