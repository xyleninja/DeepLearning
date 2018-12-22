using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTextRecognition
{
    public class SingleLayerPerceptron
    {
        public List<SLPNode> inputNodes { get; set; }
        public List<SLPNode> outputNodes { get; set; }

        public SingleLayerPerceptron(int inputs, int outputs)
        {
            for (int i = 0; i < inputs - 1; i++)
            {
                inputNodes.Add(new SLPNode());
            }

            for (int i = 0; i < outputs - 1; i++)
            {
                SLPNode outputNode = new SLPNode();
                Random random = new Random();

                foreach (SLPNode inputNode in inputNodes)
                {
                    outputNode.weights.Add(inputNode, random.Next(-1000, 1000) / 1000);
                }
                outputNodes.Add(outputNode);
            }

        }

        public bool[] predict(bool[] input)
        {
            bool[] result = new bool[outputNodes.Count];

            for (int i = 0; i < result.Length - 1; i++)
            {
                double net = 0;
                for (int j = 0; j < input.Length - 1; j++)
                {
                    if (input[j])
                        net += 1 * outputNodes[i].weights[inputNodes[j]];
                    else continue;
                }

                if (net > 0)
                    result[i] = true;
                else
                    result[i] = false;
            }

            return result;
        }

        public class SLPNode
        {
            // public double? bias { get; set; }
            public Dictionary<SLPNode, double> weights { get; set; } = new Dictionary<SLPNode, double>();
        }
    }
}
