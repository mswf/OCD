using UnityEngine;
using System.Collections;

abstract public class IVehicle : MonoBehaviour 
{
	protected const float DEFAULT_MAX_SPEED = 1f;
	protected const float DEFAULT_ACCELERATION_SPEED = 0.005f;
	protected const float FRICTION = 0.98f;
	
	protected const float MIN_SIDE_SPEED = 1f;
	protected const float MAX_SIDE_SPEED = 1.5f;
	protected const float MIN_ACCELERATION_SPEED = 1f;
	
	protected const int SHIELD_DAMAGE = 20;
	protected const int MAX_SHIELD = 100;
	protected const int MAX_POWER_POINT = 100;
	protected const int MIN_POWER_POINT = 0;
	
	protected const float GRAVITY_STRENGTH = 0.15f;
	
	protected const float RAY_DIST_SIDE = 40f;
	protected const float RAY_DIST_DOWN = 8f;
	
	
	
	protected float _currentSpeed;					//forward speed
	protected float _currentSideSpeed;				//sideways speed
	protected float _currentMaxSpeed;
	protected float _currentAccelerationSpeed;

	protected bool _isPlayer = false;
	protected int _id = 0;
	protected int _shield = 50;
	protected int _pPoints = 0;

	public bool isPlayer { get { return _isPlayer; }
						   set {_isPlayer = value; } 
						 }
	public int id { get { return _id; }
						   set {_id = value; } 
						 }
	public int shield { get { return _shield; }
						   set {_shield = value; } 
						 }
	public int pPoints { get { return _pPoints; }
						   set {_pPoints = value; } 
						 }
	public float currentSpeed { get { return _currentSpeed; } }

	protected bool _grounded;
	protected Quaternion _lookAt = Quaternion.identity;
	
	
	//functions

	abstract public void decrSpeed(float amount, float durationInSeconds);
	abstract public void incrSpeed(float amount, float durationInSeconds);
	abstract protected void resetSpeed();
	
	abstract public void decrAcc(float multiplier, float durationInSeconds);
	abstract public void incrAcc(float multiplier, float durationInSeconds);
	abstract protected void resetAcc();
	
	abstract public void incrShield(int amount);
	abstract public void drainShield(int damage);
	abstract protected void pointsToShield(int damage);
	

	/// <summary>
	/// do raycast down here and align vehicle to the normal of the road
	/// </summary>
	/// <returns>distance between vehicle and floor</returns>
	abstract protected void vehicleFloorAlignment();
	abstract protected void verticalAlignment(); //based on speed
	abstract protected void sideBoundaryUpdate();
	
	abstract protected void speedUpdate();
	abstract public void goLeft(float amount);
	abstract public void goRight(float amount);
	abstract protected void goForward();
	
	abstract protected void lookAtTowardsTo();
	
}
