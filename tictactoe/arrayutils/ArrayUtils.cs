namespace arrayutils
{
    public static class ArrayUtils
    {
        public static IEnumerable<T> GetRow<T>(this T[,] array, int rowIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }

            if (!(rowIndex >= 0 && rowIndex < array.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            for (int i = 0; i < array.GetLength(1); i++)
            {
                yield return array[rowIndex, i];
            }
        }

        public static IEnumerable<T> GetCol<T>(this T[,] array, int colIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }

            if (!(colIndex >= 0 && colIndex < array.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex), "Cannot be out of range!");
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                yield return array[i, colIndex];
            }
        }

        public static IEnumerable<T> GetDiagonal<T>(this T[,] array, int rowIndex, int colIndex, bool isRising)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }

            if (!(rowIndex >= 0 && rowIndex < array.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            if (isRising)
            {
                return array.GetRisingDiagonal(rowIndex, colIndex);
            }

            return array.GetFallingDiagonal(rowIndex, colIndex);
        }

        public static bool ContainsSequenceDuplicates<T>(this T[] array, int duplicateCount, T elToSeek)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }

            if (duplicateCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duplicateCount), "Has to be greater than 0!");
            }

            if (duplicateCount > array.Length)
            {
                return false;
            }

            if (elToSeek == null)
            {
                throw new ArgumentNullException(nameof(elToSeek), "Cannot seek for null element!");
            }

            List<T> arrayList = array.ToList();

            for (int i = 0; i < array.Length; i++)
            {
                if (i + duplicateCount - 1 < array.Length)
                {
                    var range = arrayList.GetRange(i, duplicateCount);

                    if (range.Any(el => el == null))
                    {
                        continue;
                    }

                    if (range.All(a => a.Equals(elToSeek)))
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }

            return false;
        }

        public static IEnumerable<int> GetDuplicateSequenceLeadingElementIndexes<T>(this T[] array, int duplicateCount, T elToSeek)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }

            if (duplicateCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duplicateCount), "Has to be greater than 0!");
            }

            if (!ContainsSequenceDuplicates(array, duplicateCount, elToSeek))
            {
                yield break;
            }

            List<T> arrayList = array.ToList();

            for (int i = 0; i < array.Length; i++)
            {
                if (i + duplicateCount - 1 < array.Length)
                {
                    var range = arrayList.GetRange(i, duplicateCount);

                    if (range[0] == null)
                    {
                        continue;
                    }

                    if (range.All(a => a.Equals(elToSeek)))
                    {
                        yield return i;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private static IEnumerable<T> GetRisingDiagonal<T>(this T[,] array, int rowIndex, int colIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }

            if (!(rowIndex >= 0 && rowIndex < array.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            if (!(colIndex >= 0 && colIndex < array.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            int r = rowIndex;
            int c = colIndex;

            for (int col = c; col < array.GetLength(1); col++)
            {
                if (r >= 0 && r < array.GetLength(0))
                {
                    yield return array[r, col];
                    r--;
                }
                else
                {
                    break;
                }
            }
        }

        private static IEnumerable<T> GetFallingDiagonal<T>(this T[,] array, int rowIndex, int colIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Cannot be null!");
            }


            if (!(rowIndex >= 0 && rowIndex < array.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            int r = rowIndex;
            int c = colIndex;

            for (int col = c; col < array.GetLength(1); col++)
            {
                if (r >= 0 && r < array.GetLength(0))
                {
                    yield return array[r, col];
                    r++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}