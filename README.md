# LearnToDrive

Machine learning game using Unity ML agents (tensorflow).
The aim is to make the car learn to drive without hitting walls

It's a sort of self driving car simulator, only using "sensors" for driving,
real car use also computer vision with cameras (see improvement ideas)

# Usage
In order to make this project work, i recommend following tutorials to setup Unity ML-Agent
https://github.com/Unity-Technologies/ml-agents

## Inputs variable
x value of relative position between point to reach and agent<br/>
z value of relative position between point to reach and agent<br/>

Check if hitting something at angles (2 origins) : 0, 45, 90, 135, 180, 110, 70
![Alt text](Screenshots/inputraycast.png?raw=true "Ray cast")

## Output variable
value to move on x axis<br/>
rotation

<br/><br/>

# Game

![Alt text](Screenshots/game.png?raw=true "Game screenshot 1")


# Videos
## Preview of 15 min trained agent - v0.1

[Version 0.1](https://www.youtube.com/watch?v=gEEcpYuBuBc)

## Preview of trained agent - v0.5 – 100 000 steps

[Version 0.5](https://www.youtube.com/watch?v=b91gtS0qliU)

## Preview of trained agent - v0.6 ~ 100 000 steps

[Version 0.6](https://www.youtube.com/watch?v=6817Ynku2Xg)

## Preview of trained agent - v0.9 - 678 000 steps - Handling speed now (not very well)

[Version 0.9](https://www.youtube.com/watch?v=U_c8JuvTiho)

# Improvement idea
 - Use better assets (real car, real world : roads...)
 - Use computer vision + actual kind of sensors
 - Build small car with Raspberry PI and sensors that use the model


