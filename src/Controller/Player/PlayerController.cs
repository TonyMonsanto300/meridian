using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XenWorld.Manager;
using XenWorld.Model;
using XenWorld.src.Controller.Player;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Ability;
using XenWorld.src.Service;

public class PlayerController {
    private Puppet playerPuppet;

    public bool IsCasting { get; set; } = false; // Indicates whether the player is casting
    public bool IsChoosingClass { get; set; } = false; // Tracks whether the player is in the class selection phase

    private InteractionMode currentMode = InteractionMode.None;
    public int SelectedAbilityClassIndex { get; private set; } = -1; // Tracks the selected AbilityClass index

    public PlayerController(Puppet puppet) {
        playerPuppet = puppet;
        KeyScanner.LastState = Keyboard.GetState(); // Initialize previous state
    }

    // Property to expose the player's Puppet
    public Puppet Puppet {
        get { return playerPuppet; }
    }

    // Public property to expose `currentMode`
    public InteractionMode CurrentMode {
        get { return currentMode; }
        set { currentMode = value; }
    }

    public void Update(GameTime gameTime) {
        KeyScanner.InputScan(gameTime);
    }

    public void EnterMode(InteractionMode modeType) {
        if (currentMode != InteractionMode.None) {
            ExitCurrentMode();
        }

        currentMode = modeType;

        switch (currentMode) {
            case InteractionMode.Scan:
            case InteractionMode.Mine:
                MapCursorService.HighlightAdjacentCells(currentMode);
                break;
            case InteractionMode.Attack:
                MapCursorService.HighlightAdjacentCells(currentMode, PlayerManager.Controller.Puppet.EquippedWeapon.Range);
                break;
            case InteractionMode.Cast:
                StartCasting(); // Start casting mode with class selection
                break;
            default:
                break;
        }
    }

    public void ExitCurrentMode() {
        switch (currentMode) {
            case InteractionMode.Attack:
            case InteractionMode.Scan:
            case InteractionMode.Mine:
            case InteractionMode.Cast:
                MapCursorService.ClearHighlightedCells();
                break;
            default:
                break;
        }

        currentMode = InteractionMode.None;
    }

    private void StartCasting() {
        IsCasting = true;
        IsChoosingClass = true; // Begin with AbilityClass selection
    }

    // Handles Phase 1: Selecting an Ability Class
    public void SelectAbilityClass(int index) {
        var availableClasses = Puppet.Abilities.Select(a => a.Class).Distinct().ToList();

        if (IsChoosingClass) {
            // First phase: Select an AbilityClass
            if (index > 0 && index <= availableClasses.Count) {
                SelectedAbilityClassIndex = index - 1; // Store the selected class index
                IsChoosingClass = false; // Transition to selecting specific abilities
                IsCasting = true;
            }
            // If invalid, do not change IsChoosingClass or IsCasting
        } else {
            // Phase 2: Select specific ability within the chosen class
            SelectAbilityFromClass(index);
        }
    }

    // Handles Phase 2: Selecting a Specific Ability within a Class
    public void SelectAbilityFromClass(int index) {
        if (SelectedAbilityClassIndex == -1 || IsChoosingClass) return; // Ensure a class is selected first and we're in the correct phase

        var availableClasses = Puppet.Abilities.Select(a => a.Class).Distinct().ToList();
        var selectedClass = availableClasses[SelectedAbilityClassIndex];

        var abilities = Puppet.Abilities.Where(a => a.Class == selectedClass).ToList();

        // Corrected condition
        if (index > 0 && index <= abilities.Count) {
            var chosenAbility = abilities[index - 1];

            // Use the selected ability
            UseAbility(chosenAbility);

            // Reset casting mode after the ability is used
            ResetCasting();
        }
    }

    // Resets all indices and exits casting mode
    public void ResetCasting() {
        SelectedAbilityClassIndex = -1;
        IsChoosingClass = false; // Reset to a neutral state
        IsCasting = false; // Casting is now complete
        ExitCurrentMode();
    }

    // Executes the chosen ability
    public void UseAbility(Ability ability) {
        LogRenderer.AddLogMessage($"{Puppet.Name} used {ability.Name} ({ability.Cost.Value} {ability.Cost.Type})");
        TakeTurn();
    }

    public virtual void TakeTurn() {
        EventManager.PlayerTurn();
    }
}