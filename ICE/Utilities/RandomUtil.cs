using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Utilities;

public class RandomUtil
{
    private static Random random = new Random();

    // Constructor with seed for reproducible results
    public static void RandomPointGenerator(int seed)
    {
        random = new Random(seed);
    }

    public static Vector3 GetRandomPointInBounds(float minX, float maxX, float minZ, float maxZ, float fixedY)
    {
        float randomX = (float)(random.NextDouble() * (maxX - minX) + minX);
        float randomZ = (float)(random.NextDouble() * (maxZ - minZ) + minZ);

        return new Vector3(randomX, fixedY, randomZ);
    }
}
