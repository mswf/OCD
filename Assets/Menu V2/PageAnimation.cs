using UnityEngine;
using System.Collections;

public class PageAnimation : MonoBehaviour {

	public AnimationCurve _animationTween = AnimationCurve.Linear(0,0,1f,1f);
	private float animationStatus = 0f;
	
	public Vector3 animationMovement = new Vector3(0,0,0);
	private Vector3 _origin;
	private Vector3 _translation;

	private bool slidingIn = false;
	private bool slidingOut = false;

	public float time = 1.0f;

	void Awake()
	{
		_origin = gameObject.transform.localPosition;
		//SlideIn();
	}

	public void SlideIn(){
		animationStatus = 0f;
		slidingIn = true;
	}

	public void SlideOut(){
		animationStatus = time;
		slidingOut = true;
	}

	// Update is called once per frame
	void Update () {
		if(slidingIn){
			if(animationStatus <=time)
				_slideInAnim();
			else
				slidingIn = false;
		}

		if(slidingOut){
			if(animationStatus >= 0f)
				_slideOutAnim();
			else
				slidingOut = false;
		}
	}

	private void _slideInAnim(){
		animationStatus += time * Time.deltaTime;
		
		_translation = (animationMovement * _animationTween.Evaluate(animationStatus/time));
		gameObject.transform.localPosition = _origin + _translation - animationMovement;
	}

	private void _slideOutAnim(){
		animationStatus -= time * Time.deltaTime;
		
		_translation = (animationMovement * _animationTween.Evaluate(animationStatus/time));
		gameObject.transform.localPosition = _origin - _translation + animationMovement;
	}
}
