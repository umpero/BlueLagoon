namespace BlueLagoon.Tools;

public static class NounsGradator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="firstVariant">examples: minuta, pomidor, kwiatek</param>
    /// <param name="secondVariant">examples: minuty, pomidory, kwiatki</param>
    /// <param name="thirdVariant">examples: minut, pomidorów, kwiatków</param>
    /// <returns>nouns variant</returns>
    public static string GradateTheNounAccordingToQuantity(int quantity, string firstVariant, string secondVariant, string thirdVariant)
    {
        if (quantity == 1)
            return firstVariant;

        if (quantity % 10 > 1 && quantity % 10 < 5 && !(quantity % 100 >= 10 && quantity % 100 <= 21))
            return secondVariant;

        return thirdVariant;
    }
}
