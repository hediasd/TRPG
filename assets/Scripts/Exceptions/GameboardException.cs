﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class GameboardException : Exception {

	public string Error;

	public GameboardException(string Error){
		this.Error = Error;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
