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

        public class SLPNode
        {
            // public double? bias { get; set; }
            public Dictionary<SLPNode, double> weights { get; set; } = new Dictionary<SLPNode, double>();
        }
    }
}
