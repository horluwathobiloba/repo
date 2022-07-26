using System;
using System.Security.Cryptography;


namespace ReventInject.Security
{
    public class Randomizer
    {

        /// <summary>
        /// This class can generate random passwords, which do not include ambiguous 
        /// characters, such as I, l, and 1. The generated password will be made of
        /// 7-bit ASCII symbols. Every four characters will include one lower case
        /// character, one upper case character, one number, and one special symbol
        /// (such as '%') in a random order. The password will always start with an
        /// alpha-numeric character; it will not start with a special symbol (we do
        /// this because some back-end systems do not like certain special
        /// characters in the first position).
        /// </summary>
        public partial class RandomPassword
        {
            // Define default min and max password lengths.
            private static int DEFAULT_MIN_PASSWORD_LENGTH = 6;

            private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;
            #region "Random Password Generator"
            // Define supported password characters divided into groups.
            // You can add (or remove) characters to (from) these groups.

            //private static string PASSWORD_CHARS_LCASE = "abcdefgijklmnopqrstwxyz";
            //private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            private static string PASSWORD_CHARS_NUMERIC = "0123456789";
            private static string PASSWORD_CHARS_SPECIAL = "!@#$%^&*=";
            private static string NUMERIC_SPECIAL_CHARS = "0!12@3#4$5%6^7&8*9=";
            private static string ALLOWED_CHARS = "aA!bB0cCdD@eE2fF#gG3hHiI$jJkK4lLmMnN%oO5pPqQ^rR6sS*tTuU7vVwW&xX8yY=zZ9";
            private static string NUMERIC_CHARS = "0123456789";
            private static string ALPHABET_CHARS = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";
            private static string ALPHANUMERIC_CHARS = "aA0bBcCdD1eE2fFgGh3HiIjJ4kKlLmM5nNoO6pPqQr7RsStTu8UvVwWxX9yYzZ";
            private static string ALPHANUMERIC_CHARS_UCASE = "A0B1CDE2FG3HIJK4LMN5OPQ6RST7UVW8XYZ9";
            private static string ALPHANUMERIC_CHARS_LCASE = "a0bc1def2ghi3jklm4nop5qrs6tuv7wxy8z9";
            private static string ALPHANUMERICSPECIAL_CHARS = "aA0!bBcC1dD@eE2fF#gG3hHiI$jJkKlL4mMnN%oO5pPqQ^rR6sS*tTuU7vVwW&xX8yY=zZ9";
            private static string ALPHANUMERICSPECIAL_CHARS_UCASE = "AB!0CD1E@2FGH#3IJK$4LMN%5OPQ^6RST&7UVW*8XYZ=9";
            private static string ALPHASPECIAL_CHARS = "aA!bBcCdD@eEfF#gGhHiI$jJkKlLmMnN%oOpPqQ^rRsS*tTuUvVwW&xXyY=zZ";
            #endregion
            private static string ALPHASPECIAL_CHARS_UCASE = "AB!CDE@FGH#IJK$LMN%OPQ^RST&UVW*XYZ=";

            public enum Allowed_XterType
            {
                Default = 0,
                Numeric = 1,
                Alphabets = 2,
                AlphaNumeric = 3,
                AlphaNumeric_UCase = 4,
                AlphaNumeric_LCase = 5,
                AlphaNumeric_Special = 6,
                AlphaNumeric_Special_UCase = 7,
                Numeric_Special = 8,
                Alpha_Special = 9,
                Alpha_Special_UCase = 10,
                Special = 11
            }

            private static string GenerateRandomPassword2(int minLength, int maxLength, Allowed_XterType Allowed_XterType)
            {

                Random rd = new Random();
                int len = 0;
                char[] chars = new char[minLength];

                if (minLength < maxLength)
                {
                    len = rd.Next(minLength, maxLength + 1);
                    chars = new char[len];
                }
                else if (minLength == maxLength)
                {
                    len = minLength;
                    chars = new char[minLength];
                }

                for (int i = 0; i <= len - 1; i++)
                {
                    switch (Allowed_XterType)
                    {
                        case Allowed_XterType.Numeric:
                            chars[i] = NUMERIC_CHARS[rd.Next(0, NUMERIC_CHARS.Length)];
                            break;
                        case Allowed_XterType.Alphabets:
                            chars[i] = ALPHABET_CHARS[rd.Next(0, ALPHABET_CHARS.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric:
                            chars[i] = ALPHANUMERIC_CHARS[rd.Next(0, ALPHANUMERIC_CHARS.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_UCase:
                            chars[i] = ALPHANUMERIC_CHARS_UCASE[rd.Next(0, ALPHANUMERIC_CHARS_UCASE.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_LCase:
                            chars[i] = ALPHANUMERIC_CHARS_LCASE[rd.Next(0, ALPHANUMERIC_CHARS_LCASE.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_Special:
                            chars[i] = ALPHANUMERICSPECIAL_CHARS[rd.Next(0, ALPHANUMERICSPECIAL_CHARS.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_Special_UCase:
                            chars[i] = ALPHANUMERICSPECIAL_CHARS[rd.Next(0, ALPHANUMERICSPECIAL_CHARS_UCASE.Length)];
                            break;
                        case Allowed_XterType.Numeric_Special:
                            chars[i] = NUMERIC_SPECIAL_CHARS[rd.Next(0, NUMERIC_SPECIAL_CHARS.Length)];
                            break;
                        case Allowed_XterType.Alpha_Special:
                            chars[i] = ALPHASPECIAL_CHARS[rd.Next(0, ALPHASPECIAL_CHARS.Length)];
                            break;
                        case Allowed_XterType.Alpha_Special_UCase:
                            chars[i] = ALPHASPECIAL_CHARS_UCASE[rd.Next(0, ALPHASPECIAL_CHARS_UCASE.Length)];
                            break;
                        case Allowed_XterType.Special:
                            chars[i] = PASSWORD_CHARS_SPECIAL[rd.Next(0, PASSWORD_CHARS_SPECIAL.Length)];
                            break;
                    }
                }

                return new string(chars);
            }

            private static string GenerateRandomPassword(int minLength, int maxLength, Allowed_XterType Allowed_XterType = Allowed_XterType.Default)
            {

                Random rd = new Random();
                int len = 0;
                char[] chars = new char[minLength];

                if (minLength < maxLength)
                {
                    len = rd.Next(minLength, maxLength + 1);
                    chars = new char[len];
                }
                else if (minLength == maxLength)
                {
                    len = minLength;
                    chars = new char[minLength];
                }

                for (int i = 0; i <= len - 1; i++)
                {
                    switch (Allowed_XterType)
                    {
                        case Allowed_XterType.Default:
                            chars[i] = ALLOWED_CHARS[rd.Next(0, ALLOWED_CHARS.Length)];
                            break;
                        case Allowed_XterType.Numeric:
                            chars[i] = NUMERIC_CHARS[rd.Next(0, NUMERIC_CHARS.Length)];
                            break;
                        case Allowed_XterType.Alphabets:
                            chars[i] = ALPHABET_CHARS[rd.Next(0, ALPHABET_CHARS.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric:
                            chars[i] = ALPHANUMERIC_CHARS[rd.Next(0, ALPHANUMERIC_CHARS.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_UCase:
                            chars[i] = ALPHANUMERIC_CHARS_UCASE[rd.Next(0, ALPHANUMERIC_CHARS_UCASE.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_LCase:
                            chars[i] = ALPHANUMERIC_CHARS_LCASE[rd.Next(0, ALPHANUMERIC_CHARS_LCASE.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_Special:
                            chars[i] = ALPHANUMERICSPECIAL_CHARS[rd.Next(0, ALPHANUMERICSPECIAL_CHARS.Length)];
                            break;
                        case Allowed_XterType.AlphaNumeric_Special_UCase:
                            chars[i] = ALPHANUMERICSPECIAL_CHARS[rd.Next(0, ALPHANUMERICSPECIAL_CHARS_UCASE.Length)];
                            break;
                        case Allowed_XterType.Numeric_Special:
                            chars[i] = NUMERIC_SPECIAL_CHARS[rd.Next(0, NUMERIC_SPECIAL_CHARS.Length)];
                            break;
                        case Allowed_XterType.Alpha_Special:
                            chars[i] = ALPHASPECIAL_CHARS[rd.Next(0, ALPHASPECIAL_CHARS.Length)];
                            break;
                        case Allowed_XterType.Alpha_Special_UCase:
                            chars[i] = ALPHASPECIAL_CHARS_UCASE[rd.Next(0, ALPHASPECIAL_CHARS_UCASE.Length)];
                            break;
                        case Allowed_XterType.Special:
                            chars[i] = PASSWORD_CHARS_SPECIAL[rd.Next(0, PASSWORD_CHARS_SPECIAL.Length)];
                            break;
                    }
                }
                //For i As Integer = 0 To len - 1
                //    chars[i] = ALLOWED_CHARS[rd.Next(0, ALLOWED_CHARS.Length)]
                //Next i

                return new string(chars);
            }

            public static string GetRandomPassword(int minLength, int maxLength, Allowed_XterType Allowed_XterType)
            {
                return GenerateRandomPassword(minLength, maxLength, Allowed_XterType);
            }

            public static string GetRandomPassword(int minLength, int maxLength)
            {
                return GenerateRandomPassword(minLength, maxLength);
            }

            public static string GetRandomPassword(int lenght, Allowed_XterType Allowed_XterType)
            {
                return GenerateRandomPassword(lenght, lenght, Allowed_XterType);
            }

            public static string GetRandomPassword(int lenght)
            {
                return GenerateRandomPassword(lenght, lenght);
            }

            /// <summary>
            /// Generates a random password.
            /// </summary>
            /// <returns>
            /// Randomly generated password string of characters.
            /// </returns>
            /// <remarks>
            /// The length of the generated password will be determined at
            /// random. It will be no shorter than the minimum default and
            /// no longer than maximum default.
            /// </remarks>
            public static string GetRandomPassword()
            {

                return GenerateRandomPassword(DEFAULT_MIN_PASSWORD_LENGTH, DEFAULT_MAX_PASSWORD_LENGTH);
            }

            /// <summary>
            /// Generates a random password.
            /// </summary>
            /// <param name="minLength"> Minimum password length.</param>
            /// <param name="maxLength"> Maximum password length. </param>         
            /// <returns> Randomly generated password. </returns>
            /// <remarks>
            /// The length of the generated password will be determined at
            /// random and it will fall with the range determined by the
            /// function parameters.
            /// </remarks>
            public static string GenerateXters(int minLength, int maxLength)
            {
                // Make sure that input parameters are valid.
                if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                {
                    return null;
                }

                // Create a local array containing supported password characters
                // grouped by types. You can remove character groups from this
                // array, but doing so will weaken the password strength.
                // Dim charGroups As Char()() = New Char()() {PASSWORD_CHARS_LCASE.ToCharArray(), PASSWORD_CHARS_UCASE.ToCharArray(), PASSWORD_CHARS_NUMERIC.ToCharArray(), PASSWORD_CHARS_SPECIAL.ToCharArray()}
                char[][] charGroups = new char[][] {
					PASSWORD_CHARS_NUMERIC.ToCharArray(),
					NUMERIC_CHARS.ToCharArray()
				};

                // Use this array to track the number of unused characters in each
                // character group.
                int[] charsLeftInGroup = new int[charGroups.Length];

                // Initially, all characters in each group are not used.
                for (int i = 0; i <= charsLeftInGroup.Length - 1; i++)
                {
                    charsLeftInGroup[i] = charGroups[i].Length;
                }

                // Use this array to track (iterate through) unused character groups.
                int[] leftGroupsOrder = new int[charGroups.Length];

                // Initially, all character groups are not used.
                for (int i = 0; i <= leftGroupsOrder.Length - 1; i++)
                {
                    leftGroupsOrder[i] = i;
                }

                // Because we cannot use the default randomizer, which is based on the
                // current time (it will produce the same "random" number within a
                // second), we will use a random number generator to seed the
                // randomizer.

                // Use a 4-byte array to fill it with random bytes and convert it then
                // to an integer value.
                byte[] randomBytes = new byte[4];

                // Generate 4 random bytes.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(randomBytes);

                // Convert 4 bytes into a 32-bit integer value.
                int seed = (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];

                // Now, this is real randomization.
                Random random = new Random(seed);

                // This array will hold password characters.
                char[] password = null;

                // Allocate appropriate memory for the password.
                if (minLength < maxLength)
                {
                    password = new char[random.Next(minLength, maxLength + 1)];
                }
                else
                {
                    password = new char[minLength];
                }

                // Index of the next character to be added to password.
                int nextCharIdx = 0;

                // Index of the next character group to be processed.
                int nextGroupIdx = 0;

                // Index which will be used to track not processed character groups.
                int nextLeftGroupsOrderIdx = 0;

                // Index of the last non-processed character in a group.
                int lastCharIdx = 0;

                // Index of the last non-processed group.
                int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                // Generate password characters one at a time.
                for (int i = 0; i <= password.Length - 1; i++)
                {
                    // If only one character group remained unprocessed, process it;
                    // otherwise, pick a random character group from the unprocessed
                    // group list. To allow a special character to appear in the
                    // first position, increment the second parameter of the Next
                    // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                    if (lastLeftGroupsOrderIdx == 0)
                    {
                        nextLeftGroupsOrderIdx = 0;
                    }
                    else
                    {
                        nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                    }

                    // Get the actual index of the character group, from which we will
                    // pick the next character.
                    nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                    // Get the index of the last unprocessed characters in this group.
                    lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                    // If only one unprocessed character is left, pick it; otherwise,
                    // get a random character from the unused character list.
                    if (lastCharIdx == 0)
                    {
                        nextCharIdx = 0;
                    }
                    else
                    {
                        nextCharIdx = random.Next(0, lastCharIdx + 1);
                    }

                    // Add this character to the password.
                    password[i] = charGroups[nextGroupIdx][nextCharIdx];

                    // If we processed the last character in this group, start over.
                    if (lastCharIdx == 0)
                    {
                        charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                        // There are more unprocessed characters left.
                    }
                    else
                    {
                        // Swap processed character with the last unprocessed character
                        // so that we don't pick it until we process all characters in
                        // this group.
                        if (lastCharIdx != nextCharIdx)
                        {
                            char temp = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp;
                        }
                        // Decrement the number of unprocessed characters in
                        // this group.
                        charsLeftInGroup[nextGroupIdx] -= 1;
                    }

                    // If we processed the last group, start all over.
                    if (lastLeftGroupsOrderIdx == 0)
                    {
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                        // There are more unprocessed groups left.
                    }
                    else
                    {
                        // Swap processed group with the last unprocessed group
                        // so that we don't pick it until we process all groups.
                        if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }
                        // Decrement the number of unprocessed groups.
                        lastLeftGroupsOrderIdx -= 1;
                    }
                }

                // Convert password characters into a string and return the result.
                return new string(password);
            }

            /// <summary>
            /// Generates a random password.
            /// </summary>
            /// <returns>
            /// Randomly generated password string of characters.
            /// </returns>
            /// <remarks>
            /// The length of the generated password will be determined at
            /// random. It will be no shorter than the minimum default and
            /// no longer than maximum default.
            /// </remarks>
            public static string Generate()
            {
                return GenerateXters(DEFAULT_MIN_PASSWORD_LENGTH, DEFAULT_MAX_PASSWORD_LENGTH);
            }

            /// <summary>
            /// Generates a random password of the exact length.
            /// </summary>
            /// <param name="length">
            /// Exact password length.
            /// </param>
            /// <returns>
            /// Randomly generated password.
            /// </returns>
            public static string GenerateXters(int length)
            {
                return GenerateXters(length, length);
            }
        }
    }
}



