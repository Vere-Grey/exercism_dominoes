using System.Collections.Generic;
using System.Linq;

public static class Dominoes
{
    private static IEnumerable<(int, int)> ExceptOneDomino(this IEnumerable<(int, int)> dominoes, (int, int) targetDomino)
    {
        var isDominoSkippedAlready = false;
        foreach (var domino in dominoes)
        {
            if (isDominoSkippedAlready || !domino.Equals(targetDomino))
            {
                yield return domino;
            }
            else
            {
                isDominoSkippedAlready = true;
            }
        }
    }

    private static bool IsThereCompatibleDomino(this IEnumerable<(int, int)> dominoes, (int firstNumber, int lastNumber) chain)
    {
        if (dominoes.Count().Equals(0))
        {
            return chain.firstNumber == chain.lastNumber;
        }

        var backupLastNumber = chain.lastNumber;
        foreach (var domino in dominoes)
        {
            if (chain.firstNumber.Equals(0)) //Picking first domino
            {
                chain.firstNumber = domino.Item1;
                chain.lastNumber = domino.Item2;
            }
            else if (chain.lastNumber.Equals(domino.Item1)) //Attaching domino by left side
            {
                chain.lastNumber = domino.Item2;
            }
            else if (chain.lastNumber.Equals(domino.Item2)) //Attaching domino by right side
            {
                chain.lastNumber = domino.Item1;
            }
            else
            {
                continue; //We cannot attach this domino
            }

            var remainingDominoes = dominoes.ExceptOneDomino(domino);

            if (remainingDominoes.IsThereCompatibleDomino(chain))
            {
                return true; //Child is returning success to parent and that will "bubble up" to the root    
            }

            chain.lastNumber = backupLastNumber; //Child returned false. We will remove last domino and try another combination.
        }


        return false; //Dead-end. We cannot attach any tiles in this recursion level. Returning to parent.
    }

    public static bool CanChain(IEnumerable<(int, int)> dominoes) => dominoes.IsThereCompatibleDomino((0, 0));
}