using UnityEngine;

public class Extinguisher : ExtinguischerModuleBehaviour {

    [SerializeField]
    private ParticleSystem emitter;

    public override void Interact(ArmToolModule module) {
        emitter.Emit(10);
    }

    private void OnParticleCollision(GameObject other) {
		if (other.tag == "Fire") {
			Destroy(other.gameObject);
		}
        else if (other.tag == "FlammableFire") {
            other.SendMessageUpwards("Extinguishing", true);
            other.gameObject.SetActive(false);
        }
	}
}
