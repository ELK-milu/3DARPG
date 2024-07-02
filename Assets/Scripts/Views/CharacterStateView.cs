using Architecture;

public class CharacterStateView : MvcView
{
    public LiquidValueState[] LiquidValueStates;
    void Start()
    {
        LiquidValueStates[0].CharacterStatesEventBinding.Add((characterStatesEvent) =>
        {
            LiquidValueStates[0].LiquidBall.DOFill(characterStatesEvent.HealthCost);
        });
        LiquidValueStates[1].CharacterStatesEventBinding.Add((characterStatesEvent) =>
        {
            LiquidValueStates[1].LiquidBall.DOFill(characterStatesEvent.ManaCost);
        });
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}