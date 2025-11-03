public class Individual
{
    public double[] Weights { get; set; }
    public double Fitness { get; set; }

    public Individual(int featureCount)
    {
        Weights = new double[featureCount];
    }
}

private double RunGenetic(int generations, int populationSize, double crossoverRate, double mutationRate)
{
    Random rand = new Random();
    List<Individual> population = InitializePopulation(populationSize, FeatureCount, rand);
    Individual best = null;
    double bestFitness = double.MaxValue;

    for (int gen = 0; gen < generations; gen++)
    {
        foreach (var ind in population)
            ind.Fitness = CalculateFitness(ind.Weights);
        population = population.OrderBy(i => i.Fitness).ToList();

        if (population[0].Fitness < bestFitness)
        {
            bestFitness = population[0].Fitness;
            best = Clone(population[0]);
        }

        var newGen = new List<Individual>();
        while (newGen.Count < populationSize)
        {
            var parent1 = TournamentSelect(population, rand);
            var parent2 = TournamentSelect(population, rand);

            Individual child = rand.NextDouble() < crossoverRate
                ? Crossover(parent1, parent2, rand)
                : Clone(parent1);

            Mutate(child, mutationRate, rand);
            newGen.Add(child);
        }

        population = newGen;
    }

    lblResults.Text += $"<hr><b>Genetic Algorithm:</b> MSE = {bestFitness:F4}<br>";
    return bestFitness;
}
