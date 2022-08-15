# Snake Battle Ground

The entire project is arquitectured in something I call by **FMVC**, that is basically a implementation of the well-known MVC that works with Facade design pattern to facilitate portability and future changes.

## Main Concepts
**Entity:** Represents anything at application that requires a MVC block (Ex: Snake, block, match, etc), being the project's organization made much in view of these entities. They usually have only one model and one view each but this is not a rule, there are cases of entities that are more complex and demand multiple of these elements. It's also important to inform that there are some loose models and view that don't need to be represented as entities because they are too "small" (Ex: PathfindingModel, CameraShake, etc).

**MVC:** Its implementation is totally inspired by a approach taken by [Jackson Dunstan](https://www.jacksondunstan.com/articles/3092), where no external framework is needed. Basically the Models here represent the business logic of a entity while the Views are responsible for the output(visual and audio) and inputs that come from Unity's callbacks (E: Update, OnTriggerEnter, etc). The controller handles the communication between those two, using Events to so.

**Facade:** It works as a wrap and hides all the "complexity" from the entities facilitating the communication between them. Each facade takes care of the internal operation of its respective entities, which includes initialize and exxpose fields and methods needed by "outsiders", making it easier for possible replacements of the MVC for something else because the external entities will communicate with each other of the same way.

## Extra concepts
**Container:** Its only objective is to resolve all the dependencies of the game, being used most of time to resolve internal dependencies of each entity (Ex: Resolve Model and View to the Snake's controller). In this project we use Extenject, a Dependency Injection framework that works well with Unity's structure. Usually each entity has its own container in order to be portable, being "self-resolvable" wherever they are placed.

**Data:** Represents all the customizable data of an entity. Here all the data are represented through ScriptableObjects to make design changes easier, but as these values only involve primitive values (ints, string, etc) they can be easily migrated to other formats like Json and XML.

##Diagram
Below is a diagram(not UML) that briefly illustrates the structure of the project:

![full-diagram-white](https://user-images.githubusercontent.com/22356981/184716896-00ac7423-6438-4913-a51c-4b468fd7a9eb.png)
