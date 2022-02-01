using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Wander : MonoBehaviour
{
	public float speed = 1;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 180;

	CharacterController controller;
    private Animator animator;
	float heading;
	Vector3 targetRotation;
	private bool active = false;

	public bool infected = false;

	void Awake ()
	{
		controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("Moving", true);
		// Set random initial rotation
		heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, heading, 0);

		
	}

	void Update ()
	{
		if (active) {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);

			float x = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
			float z = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);
			controller.Move(movement.noramlized * Time.deltaTime * speed);
		}
	}

	/// <summary>
	/// Repeatedly calculates a new direction to move towards.
	/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
	/// </summary>
	IEnumerator NewHeading ()
	{
		while (true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	/// <summary>
	/// Calculates a new direction to move towards.
	/// </summary>
	void NewHeadingRoutine ()
	{
		var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
		var ceil  = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
		heading = Random.Range(floor, ceil);
		targetRotation = new Vector3(0, heading, 0);
	}

	public void Activate() {
		active = true;
		StartCoroutine(NewHeading());
	}

	public void Deactivate() {
		active = false;
	}
}