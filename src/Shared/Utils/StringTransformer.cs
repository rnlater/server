namespace Shared.Utils;

public class StringTransformer
{
    public static string? GetShuffledVersion(string input)
    {
        var words = input.Split(" ");
        if (words.Length == 1)
        {
            if (words[0].Length == 1)
                return null;
            var chars = words[0].ToCharArray();
            var random = new Random();
            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }
            return new string(chars);
        }
        else
        {
            var random = new Random();
            for (int i = words.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = words[i];
                words[i] = words[j];
                words[j] = temp;
            }
            return string.Join(" ", words);

        }
    }

    public static string GetBlankedVersion(string input)
    {
        var words = input.Split(" ");
        if (words.Length == 1)
        {
            return ApplyUnderscore(words[0]);
        }
        else if (words.Length >= 2 && words.Length <= 3)
        {
            var random = new Random();
            var index = random.Next(words.Length);
            words[index] = ApplyUnderscore(words[index]);
            return string.Join(' ', words);
        }
        else if (words.Length > 3)
        {
            var random = new Random();
            var index = random.Next(words.Length);
            words[index] = new string('_', words[index].Length);
            return string.Join(' ', words);
        }

        return input;
    }

    private static string ApplyUnderscore(string word)
    {
        if (word.Length >= 1 && word.Length <= 4)
        {
            var random = new Random();
            var index = word.Length == 1 ? 0 : random.Next(1, word.Length - 1);
            var chars = word.ToCharArray();
            chars[index] = '_';
            var underscoredWord = new string(chars);

            return underscoredWord;
        }
        else
        {
            var random = new Random();
            var chars = word.ToCharArray();
            var index1 = random.Next(1, word.Length - 1);
            var index2 = random.Next(1, word.Length - 1);

            while (index2 == index1)
            {
                index2 = random.Next(1, word.Length - 1);
            }

            chars[index1] = '_';
            chars[index2] = '_';
            var underscoredWord = new string(chars);

            return underscoredWord;
        }
    }

}