# Dotmim.Sync.FKError
This example shows filters crossing scopes can cause foreign key errors while inserting.  If this happens sync will never finish.

The question will be why not use 1 scope with an in clause for all the parameters?  Well what happens when the parameter list changes and you need to get historical data for the new parameters?  Or you need to cleanup data that is not relevant anymore.  The scope tracks the row version so if a new parameter was added it would only get data from the current time onward.  Creating an instance of a scope per filter solves this and sync framework was able to handle the foreign key constraint errors.

A real life scenario is user -> supervisor which means filters are dynamic based on the user.  if an existing employee is reporting to a new supervisor then the historical data of that employee needs to sync to the supervisor.  If 1 scope is used this is not possible unless the scope is re-initialized with upload, but that syncs a bunch of data already existing on the client.
