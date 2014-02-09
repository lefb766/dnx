using System;
using System.Collections.Generic;

namespace klr.hosting
{
    internal class CommandOptions
    {
        private readonly Dictionary<string, List<string>> _parsedOptions;
        private readonly Dictionary<string, CommandOptionType> _validOptions;

        internal CommandOptions(Dictionary<string, CommandOptionType> validOptions,
                              Dictionary<string, List<string>> parsedOptions,
                              IList<string> remainingArgs)
        {
            _validOptions = validOptions;
            _parsedOptions = parsedOptions;
            RemainingArgs = remainingArgs;
        }

        internal IList<string> RemainingArgs { get; private set; }

        internal bool HasOption(string option)
        {
            return _parsedOptions.ContainsKey(option);
        }

        internal string GetValue(string option)
        {
            List<string> values;
            if (_parsedOptions.TryGetValue(option, out values))
            {
                switch (_validOptions[option])
                {
                    case CommandOptionType.MultipleValue:
                    case CommandOptionType.SingleValue:
                        return values[0];
                    case CommandOptionType.NoValue:
                        throw new InvalidOperationException(option + " does not allow values");
                }
            }

            return null;
        }

        internal IEnumerable<string> GetValues(string option)
        {
            List<string> values;
            if (_parsedOptions.TryGetValue(option, out values))
            {
                switch (_validOptions[option])
                {
                    case CommandOptionType.MultipleValue:
                        return values;
                    case CommandOptionType.SingleValue:
                    case CommandOptionType.NoValue:
                        throw new InvalidOperationException(option + " does not allow multiple values");
                }
            }

            return null;
        }
    }
}
