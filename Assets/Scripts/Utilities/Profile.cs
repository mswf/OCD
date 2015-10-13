using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Profile {
	
	private ProfileHandler xml = new ProfileHandler(); 
	
	public void createProfile(string _name, Axis _axis, Quaternion _left, Quaternion _right)
	{	
		xml.saveProfile(_name, _left.eulerAngles, _right.eulerAngles, _axis);
		
	}
	
	public void loadProfile(int id)
	{
		if (xml.loadProfile(id))
		{
			//ID of the profile
			PlayerPrefs.SetInt("ID", id);
			
			//Name of the profile
			PlayerPrefs.SetString("Name", xml.getName());
				
			//Axis of the profile
			PlayerPrefs.SetString("Axis", xml.getAxis());
			
			//Assigns floats for the left orientation quarternion
			PlayerPrefs.SetFloat("QLeftX", xml.getLeft().x);
			PlayerPrefs.SetFloat("QLeftY", xml.getLeft().y);
			PlayerPrefs.SetFloat("QLeftZ", xml.getLeft().z);
			
			//Assigns floats for the right orientation quarternion
			PlayerPrefs.SetFloat("QRightX", xml.getRight().x);
			PlayerPrefs.SetFloat("QRightY", xml.getRight().y);
			PlayerPrefs.SetFloat("QRightZ", xml.getRight().z);
		}
	}
	
	public void updateCurrentProfile(Axis _axis ,Quaternion _left, Quaternion _right)
	{
		//Assigns string for the axis
		PlayerPrefs.SetString("Axis", _axis.ToString());
		
		//Assigns floats for the left orientation quarternion
		PlayerPrefs.SetFloat("QLeftX", _left.eulerAngles.x);
		PlayerPrefs.SetFloat("QLeftY", _left.eulerAngles.y);
		PlayerPrefs.SetFloat("QLeftZ", _left.eulerAngles.z);
			
		//Assigns floats for the right orientation quarternion
		PlayerPrefs.SetFloat("QRightX", _right.eulerAngles.x);
		PlayerPrefs.SetFloat("QRightY", _right.eulerAngles.y);
		PlayerPrefs.SetFloat("QRightZ", _right.eulerAngles.z);
	}
	
	public int getProfileID()
	{
		return PlayerPrefs.GetInt("ID");
	}
	
	public string getProfileName()
	{
		return PlayerPrefs.GetString("Name");
	}
	
	public Quaternion getLeft()
	{
		return Quaternion.Euler(PlayerPrefs.GetFloat("QLeftX"),PlayerPrefs.GetFloat("QLeftY"),PlayerPrefs.GetFloat("QLeftZ"));
	}
	
	public Quaternion getRight()
	{
		return Quaternion.Euler(PlayerPrefs.GetFloat("QRightX"),PlayerPrefs.GetFloat("QRightY"),PlayerPrefs.GetFloat("QRightZ"));
	}
	
	public Axis getAxis()
	{
		string tempAxis = PlayerPrefs.GetString("Axis");
		switch(tempAxis)
		{
			case "x": return Axis.x;
			case "y": return Axis.y;
			case "z": return Axis.z;
			default : return Axis.y;
		}
	}
	
	public List<ProfileHandler.PlayerName> getList()
	{
		return xml.getNames();
	}
	
	public void updateProfile()
	{
		xml.saveProfile(getProfileName(), getLeft().eulerAngles, getRight().eulerAngles, getAxis(), getProfileID());
	}
	
}
