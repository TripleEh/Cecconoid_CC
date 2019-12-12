// Callback.cs v0.1 (20090925) by Rod Hyde (badlydrawnrod).
//
// These are callbacks (delegates) that can be used by the messengers defined
// in Messenger.cs.

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);