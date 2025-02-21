namespace Dsm.Loaders;

public class WildcardMatcher
{
    public static bool Match(string input, string pattern)
    {
        int inputIndex = 0, patternIndex = 0;
        int inputStar = -1, patternStar = -1;

        while (inputIndex < input.Length)
        {
            if (patternIndex < pattern.Length && 
                (pattern[patternIndex] == '?' || 
                 char.ToLower(input[inputIndex]) == char.ToLower(pattern[patternIndex])))
            {
                inputIndex++;
                patternIndex++;
            }
            else if (patternIndex < pattern.Length && pattern[patternIndex] == '*')
            {
                inputStar = inputIndex;
                patternStar = patternIndex++;
            }
            else if (patternStar != -1)
            {
                inputIndex = ++inputStar;
                patternIndex = patternStar + 1;
            }
            else
            {
                return false;
            }
        }

        while (patternIndex < pattern.Length && pattern[patternIndex] == '*')
        {
            patternIndex++;
        }

        return patternIndex == pattern.Length;
    }
}