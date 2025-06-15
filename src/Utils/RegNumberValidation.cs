using System.Text.RegularExpressions;

namespace eQuantic.Core.Utils;

/// <summary>
/// Provides validation methods for registration numbers.
/// </summary>
public static class RegNumberValidation
{
    /// <summary>
    /// Validates a Brazilian CPF (Cadastro de Pessoas Físicas) number.
    /// </summary>
    /// <param name="cpf">The CPF number to validate.</param>
    /// <returns>True if the CPF is valid; otherwise, false.</returns>
    public static bool CpfIsValid(string cpf)
    {
        Regex reg = new Regex(@"[^0-9]");
        cpf = reg.Replace(cpf, string.Empty);

        if (cpf.Length > 11)
            return false;

        while (cpf.Length != 11)
            cpf = '0' + cpf;

        bool equal = true;
        for (int i = 1; i < 11 && equal; i++)
            if (cpf[i] != cpf[0])
                equal = false;

        if (equal || cpf == "12345678909")
            return false;

        int[] numbers = new int[11];

        for (int i = 0; i < 11; i++)
            numbers[i] = int.Parse(cpf[i].ToString());

        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (10 - i) * numbers[i];

        int result = sum % 11;

        if (result == 1 || result == 0)
        {
            if (numbers[9] != 0)
                return false;
        }
        else if (numbers[9] != 11 - result)
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (11 - i) * numbers[i];

        result = sum % 11;

        if (result == 1 || result == 0)
        {
            if (numbers[10] != 0)
                return false;
        }
        else
        if (numbers[10] != 11 - result)
            return false;

        return true;
    }
}