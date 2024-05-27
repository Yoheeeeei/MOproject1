using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_test9
{
    internal class Calculation
    {
        //転置行列を作る関数です。https://qiita.com/sekky0816/items/8c73a7ec32fd9b040127
        double[,] Transpose(double[,] A) 
        {

            double[,] AT = new double[A.GetLength(1), A.GetLength(0)];

            for (int i = 0; i < A.GetLength(1); i++)
            {
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    AT[i, j] = A[j, i];
                }
            }
            return AT;
        }

        //逆行列を作る関数です。
        double[,] inverseMatrix(double[,] A)
        {

            int n = A.GetLength(0);
            int m = A.GetLength(1);

            double[,] invA = new double[n, m];

            if (n == m)
            {

                int max;
                double tmp;

                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        invA[j, i] = (i == j) ? 1 : 0;
                    }
                }

                for (int k = 0; k < n; k++)
                {
                    max = k;
                    for (int j = k + 1; j < n; j++)
                    {
                        if (Math.Abs(A[j, k]) > Math.Abs(A[max, k]))
                        {
                            max = j;
                        }
                    }

                    if (max != k)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            // 入力行列側
                            tmp = A[max, i];
                            A[max, i] = A[k, i];
                            A[k, i] = tmp;
                            // 単位行列側
                            tmp = invA[max, i];
                            invA[max, i] = invA[k, i];
                            invA[k, i] = tmp;
                        }
                    }

                    tmp = A[k, k];

                    for (int i = 0; i < n; i++)
                    {
                        A[k, i] /= tmp;
                        invA[k, i] /= tmp;
                    }

                    for (int j = 0; j < n; j++)
                    {
                        if (j != k)
                        {
                            tmp = A[j, k] / A[k, k];
                            for (int i = 0; i < n; i++)
                            {
                                A[j, i] = A[j, i] - A[k, i] * tmp;
                                invA[j, i] = invA[j, i] - invA[k, i] * tmp;
                            }
                        }
                    }

                }


                //逆行列が計算できなかった時の措置
                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (double.IsNaN(invA[j, i]))
                        {
                            Console.WriteLine("Error : Unable to compute inverse matrix");
                            invA[j, i] = 0;//ここでは，とりあえずゼロに置き換えることにする
                        }
                    }
                }


                return invA;

            }
            else
            {
                Console.WriteLine("Error : It is not a square matrix");
                return invA;
            }

        }

        //行列どうしの積を計算する関数です。
        double[,] MatrixTimesMatrix(double[,] A, double[,] B)
        {

            double[,] product = new double[A.GetLength(0), B.GetLength(1)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    for (int k = 0; k < A.GetLength(1); k++)
                    {
                        product[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return product;

        }

        //最小二乗法を疑似逆行列を用いて解決します。
        public double approximation(double[,] A, double[,] B)
        {

            //Aの転置At, (A*At)の逆行列Ainを求めます。
            double[,] At = Transpose(A);
            double[,] Ain = inverseMatrix(MatrixTimesMatrix(At, A));

            //Ain*At*B が答えです。

            double[,] matrix_ans = MatrixTimesMatrix(MatrixTimesMatrix(Ain, At), B);

            //最下点を見つけます。y = ax^2 + bx +cの微分です。

            double product = -matrix_ans[1, 0] / matrix_ans[0, 0];

            return product;
        }

        //こんなかんじで使います
        //Calculation calculation = new Calculation();

        //double[,] ans;
        //double[,] data1 = new double[11, 3] {//x^2, x, 1
        //    {0,0, 1},
        //    {1,1,1 },
        //    {4, 2,1},
        //    {9,3, 1},
        //    {16,4, 1},
        //    {25,5, 1},
        //    {1,-1, 1},
        //    {4,-2, 1},
        //    {9,-3, 1 },
        //    {16,-4, 1},
        //    {25,-5,1}
        //    };

        //double[,] data2 = new double[11, 1] { { 0 }, { 1 }, { 4 }, { 9 }, { 16 }, { 25 }, { 1 }, { 4 }, { 9 }, { 16 }, { 25 } };

        //ans = calculation.approximation(data1, data2);

        //    Console.WriteLine(ans[0, 0]);
        //    Console.WriteLine(ans[1, 0]);
        //    Console.WriteLine(ans[2, 0]);
    }
}
