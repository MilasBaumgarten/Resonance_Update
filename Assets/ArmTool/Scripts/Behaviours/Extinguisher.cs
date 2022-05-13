using UnityEngine;

public class Extinguisher : ArmToolModuleBehaviour {

    [SerializeField]
    private ParticleSystem emitter;

    public override void Interact(ArmToolModule module) {
        emitter.Emit(10);
    }

    private void OnParticleCollision(GameObject other) {
		if (other.CompareTag("Fire")) {
			Destroy(other.gameObject);
		}
        else if (other.CompareTag("FlammableFire")) {
            other.SendMessageUpwards("Extinguishing", true);
            other.gameObject.SetActive(false);
        }
	}
}
