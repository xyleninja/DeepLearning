using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTextRecognition
{
    public class SingleLayerPerceptron
    {
        public List<SLPNode> inputNodes { get; set; } = new List<SLPNode>();
        public List<SLPNode> outputNodes { get; set; } = new List<SLPNode>();

        public SingleLayerPerceptron(int inputs, int outputs)
        {
            for (int i = 0; i < inputs; i++)
            {
                inputNodes.Add(new SLPNode());
            }

            for (int i = 0; i < outputs; i++)
            {
                SLPNode outputNode = new SLPNode();
                Random random = new Random();

                foreach (SLPNode inputNode in inputNodes)
                {
                    outputNode.weights.Add(inputNode, random.Next(-1000, 1000) * 0.001);
                }
                outputNodes.Add(outputNode);
            }

        }

        public int predict(bool[] input)
        {
            bool[] result = new bool[outputNodes.Count];

            double maxOutput = 0;
            int maxIndex = 0;

            for (int i = 0; i < result.Length - 1; i++)
            {
                double nodeResult = calcNodeOutput(outputNodes[i], input);

                if (nodeResult > maxOutput)
                {
                    maxOutput = nodeResult;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        internal void train(bool[] input, bool[] output)
        {
            foreach (SLPNode outputNode in outputNodes)
            {
                double nodeOutput = calcNodeOutput(outputNode, input);
                double nodeError = getNodeError(nodeOutput, Convert.ToInt32(output[outputNodes.IndexOf(outputNode)]));
                updateNodeWeights(outputNode,input,nodeError);
            }
        }

        private void updateNodeWeights(SLPNode outputNode, bool[] input, double nodeError)
        {
            double LEARNING_RATE = 0.1;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i])
                    outputNode.weights[inputNodes[i]] += LEARNING_RATE * nodeError;
            }
        }

        private double getNodeError(double nodeOutput, int target)
        {
            return target - nodeOutput;
        }

        internal double calcNodeOutput(SLPNode node, bool[] input)
        {
            double result = 0;

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i])
                    result += 1 * node.weights[inputNodes[i]];
            }

            return result /= 16 * 16;
        }

        public class SLPNode
        {
            // public double? bias { get; set; }
            public Dictionary<SLPNode, double> weights { get; set; } = new Dictionary<SLPNode, double>();
        }
    }
}
