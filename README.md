# Virus-Simulation

This project was created for the 2022 Szegedi innovation competition. It is written in Unity so it can visualize the viruses spreading. The project aims to simulate a small community's daily life while one or several viruses are trying to infect the population.

The population varies by age, so there are children who needs to go to school (displayed with a capital S in the program and green background). And there are adults who needs to go to work (displayed with a capital W in the program and orange background) and their shift varies. The households also need to go to the market (displayed with a capital M and blue background) when their supplies are running low. Every family has it's own house (displayed with a capital H and brown background) where they return after work or school.

In the main menu the user can alter the simulation's base parameters.
-The world size controls how big the simulation should be.
-The building density controls how many buildings should be. The lower the less chance for it to spawn a building to the specified spot.
-The building necessity controls how big percentage of the workplaces will be mandatory to the society to these places the people will still go to work even during a lockdown.
-The anti vaccination rate is how big of a percentage of the population will be against the vaccination and won't be willing to get it.
-The supply amount is how many days could a family stock up in single market run. This is divided by the number of family members.
-The minimum family size (there must be at least one adult).
-The maximum family size.
-The virus variance how much can the virus evolve from person to person.
-The families to infect on start is how many families will get the virus on the simulation startup.

The simulation can also be altered during the simulation with the top right controls. Such as increase the speed of the simulation, virus's potency, issue a lockdown or how many vaccines are issued daily. This is also where the user can go back to the main menu.

On the top left there is a detailed statistics about the simulation. Such as how many days have been elapsed, is lockdown is in effect or the number of people categorized.
Theese values are also exported in a .csv daily intervals.

On the bottom left there is a graph which shows the number of infected people in the last 10 days.

If you wish to add other viruses or modify the current one you can do that with the VirusFileCreator.exe which is located in the VirusFileCreator folder.

Controls:
WASD movement and +/- zoom
