using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
public static class Function
{
    public static int FindRoot(int[] parents, int index)
    {
        if (index != parents[index])
            parents[index] = FindRoot(parents, parents[index]);
        return parents[index];
    }

    public static void Union(int[] parents, int from, int to)
    {
        int r1 = FindRoot(parents, from);
        int r2 = FindRoot(parents, to);
        if (r1 != r2)
            parents[r1] = r2;
    }

    // ─────────── 1) 완전 연결 그룹화 ───────────
    public static List<T[]> CreateUnion<T>(
        T[] input,
        Func<T, T, bool> is_connect
    )
    {
        int n = input.Length;
        int[] parents = new int[n];
        for (int i = 0; i < n; i++)
            parents[i] = i;

        // 1) Union 수행 (O(n²))
        for (int i = 0; i < n; i++)
        {
            if (input[i] == null)
            {
                parents[i] = -1;
                continue;
            }
            for (int j = i + 1; j < n; j++)
            {
                if (input[j] == null) continue;
                if (is_connect(input[i], input[j]))
                    Union(parents, j, i);
            }
        }

        // 2) 완전 경로 압축
        for (int i = 0; i < n; i++)
            if (parents[i] >= 0)
                parents[i] = FindRoot(parents, i);

        // 3) 루트별로 그룹화
        var map = new Dictionary<int, List<T>>();
        for (int i = 0; i < n; i++)
        {
            if (parents[i] < 0) continue;
            int root = parents[i];
            if (!map.ContainsKey(root))
                map[root] = new List<T>();
            map[root].Add(input[i]);
        }

        // 4) 결과 생성
        var result = new List<T[]>();
        foreach (var group in map.Values)
            result.Add(group.ToArray());

        return result;
    }

    // ─────────── 2) 인접 조건만 연결 ───────────

    public static List<T[]> CreateUnionConditionNext<T>(
        T[] input,
        Func<T, T, bool> is_connect
    )
    {
        int n = input.Length;
        int[] parents = new int[n];
        for (int i = 0; i < n; i++)
            parents[i] = i;

        // 이웃만 검사
        for (int i = 0; i < n; i++)
        {
            if (input[i] == null)
            {
                parents[i] = -1;
                continue;
            }
            if (i + 1 < n && input[i + 1] != null && is_connect(input[i], input[i + 1]))
                Union(parents, i, i + 1);
            if (i - 1 >= 0 && input[i - 1] != null && is_connect(input[i], input[i - 1]))
                Union(parents, i, i - 1);
        }

        // 경로 압축 + 그룹화 (위와 동일)
        for (int i = 0; i < n; i++)
            if (parents[i] >= 0)
                parents[i] = FindRoot(parents, i);

        var map = new Dictionary<int, List<T>>();
        for (int i = 0; i < n; i++)
        {
            if (parents[i] < 0) continue;
            int root = parents[i];
            if (!map.ContainsKey(root))
                map[root] = new List<T>();
            map[root].Add(input[i]);
        }

        var result = new List<T[]>();
        foreach (var group in map.Values)
            result.Add(group.ToArray());

        return result;
    }

    // ─────────── 3) 마스크 배열 형태로 반환 ───────────

    public static List<T[]> CreateUnionMaskConditionNext<T>(
        T[] input,
        Func<T, T, bool> is_connect
    )
    {
        int n = input.Length;
        int[] parents = new int[n];
        for (int i = 0; i < n; i++)
            parents[i] = i;

        // 이웃만 검사
        for (int i = 0; i < n; i++)
        {
            if (input[i] == null)
            {
                parents[i] = -1;
                continue;
            }
            if (i + 1 < n && input[i + 1] != null && is_connect(input[i], input[i + 1]))
                Union(parents, i, i + 1);
            if (i - 1 >= 0 && input[i - 1] != null && is_connect(input[i], input[i - 1]))
                Union(parents, i, i - 1);
        }

        // 경로 압축
        for (int i = 0; i < n; i++)
            if (parents[i] >= 0)
                parents[i] = FindRoot(parents, i);

        // 루트별로 “마스크 배열” 생성
        var map = new Dictionary<int, T[]>();
        for (int i = 0; i < n; i++)
        {
            if (parents[i] < 0) continue;
            int root = parents[i];
            if (!map.ContainsKey(root))
                map[root] = new T[n];  // new T[n] 은 자동으로 default(T)=null 로 초기화됩니다
            map[root][i] = input[i];
        }

        var result = new List<T[]>();
        foreach (var mask in map.Values)
            result.Add(mask);

        return result;
    }
    public static List<T[]> CreateUnion<T>(
    T[,] input,
    Func<T, T, bool> is_connect
)
    {
        int rows = input.GetLength(0);
        int cols = input.GetLength(1);
        int n = rows * cols;
        int[] parents = new int[n];

        // 1) 초기화
        for (int i = 0; i < n; i++)
            parents[i] = i;

        // 2) 모든 쌍에 대해 연결 검사 (O(n^2))
        for (int a = 0; a < n; a++)
        {
            int ax = a / cols;
            int ay = a % cols;
            T va = input[ax, ay];
            if (va == null)
            {
                parents[a] = -1;  // 마스킹
                continue;
            }

            for (int b = a + 1; b < n; b++)
            {
                int bx = b / cols;
                int by = b % cols;
                T vb = input[bx, by];
                if (vb == null) continue;

                if (is_connect(va, vb))
                    Union(parents, b, a);
            }
        }

        // 3) 완전 경로 압축
        for (int i = 0; i < n; i++)
            if (parents[i] >= 0)
                parents[i] = FindRoot(parents, i);

        // 4) 루트별로 그룹 모으기
        var map = new Dictionary<int, List<T>>();
        for (int i = 0; i < n; i++)
        {
            if (parents[i] < 0) continue;

            int root = parents[i];
            if (!map.ContainsKey(root))
                map[root] = new List<T>();

            int x = i / cols;
            int y = i % cols;
            map[root].Add(input[x, y]);
        }

        // 5) 결과 배열로 변환
        var result = new List<T[]>();
        foreach (var group in map.Values)
            result.Add(group.ToArray());

        return result;
    }
    public static List<T[]> CreateUnionConditionNext<T>(
    T[,] input,
    Func<T, T, bool> is_connect
    )
    {
        int rows = input.GetLength(0);
        int cols = input.GetLength(1);
        int n = rows * cols;
        int[] parents = new int[n];

        // 1) 초기화
        for (int i = 0; i < n; i++)
            parents[i] = i;

        // 2) 인접(우, 아래)만 검사하여 Union
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int idx = x * cols + y;
                T curr = input[x, y];
                if (curr == null)
                {
                    parents[idx] = -1;
                    continue;
                }

                // 오른쪽 이웃
                if (y + 1 < cols)
                {
                    T right = input[x, y + 1];
                    if (right != null && is_connect(curr, right))
                        Union(parents, idx, x * cols + (y + 1));
                }

                // 아래 이웃
                if (x + 1 < rows)
                {
                    T down = input[x + 1, y];
                    if (down != null && is_connect(curr, down))
                        Union(parents, idx, (x + 1) * cols + y);
                }
            }
        }

        // 3) 완전 경로 압축
        for (int i = 0; i < n; i++)
            if (parents[i] >= 0)
                parents[i] = FindRoot(parents, i);

        // 4) 루트별로 그룹화
        var map = new Dictionary<int, List<T>>();
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int idx = x * cols + y;
                if (parents[idx] < 0) continue;

                int root = parents[idx];
                if (!map.ContainsKey(root))
                    map[root] = new List<T>();

                map[root].Add(input[x, y]);
            }
        }

        // 5) 결과 반환
        var result = new List<T[]>();
        foreach (var group in map.Values)
            result.Add(group.ToArray());

        return result;
    }

    public static List<T[,]> CreateUnionMaskConditionNext<T>(
    T[,] input,
    Func<T, T, bool> is_connect
    )
    {
        int rows = input.GetLength(0);
        int cols = input.GetLength(1);
        int n = rows * cols;
        int[] parents = new int[n];

        // 1) 초기화
        for (int i = 0; i < n; i++)
            parents[i] = i;

        // 2) 인접(우, 아래) 검사하여 Union
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int idx = x * cols + y;
                T curr = input[x, y];
                if (curr == null)
                {
                    parents[idx] = -1;
                    continue;
                }

                // 오른쪽 이웃
                if (y + 1 < cols)
                {
                    T right = input[x, y + 1];
                    if (right != null && is_connect(curr, right))
                        Union(parents, idx, x * cols + (y + 1));
                }

                // 아래 이웃
                if (x + 1 < rows)
                {
                    T down = input[x + 1, y];
                    if (down != null && is_connect(curr, down))
                        Union(parents, idx, (x + 1) * cols + y);
                }
            }
        }

        // 3) 완전 경로 압축
        for (int i = 0; i < n; i++)
            if (parents[i] >= 0)
                parents[i] = FindRoot(parents, i);

        // 4) 루트별로 “마스크” 2D 배열 생성
        var map = new Dictionary<int, T[,]>();
        foreach (int i in Enumerable.Range(0, n))
        {
            if (parents[i] < 0) continue;
            int root = parents[i];

            if (!map.ContainsKey(root))
                map[root] = new T[rows, cols];  // 자동으로 default(T)=null 로 초기화

            int x = i / cols;
            int y = i % cols;
            map[root][x, y] = input[x, y];
        }

        // 5) 결과 반환
        return map.Values.ToList();
    }
}
