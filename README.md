# JSBuilder

JSBuilder is a preprocessor designed to combine multiple JavaScript files before deployment, thus avoiding the run-time overhead of including each file separately.

**Note**: I no longer plan to continue the development of this project.
I'm now using [Browserify](//github.com/substack/node-browserify) to combine JavaScript files.
Although it incurs a small overhead at the start, it uses the more elegant node module interface.

## How to build

Before using the JSBuilder, you'll need to build it using Visual C# Express 2010 (or compatible tool).
Load the project into the IDE and select _Build Solution_ from the _Build_ menu.
The executable is located in the `bin` folder inside the project folder.

## How to use

To combine multiple JavaScript files, create a main file that contains requires all the other files.
Then execute JSBuilder, passing the name of the main file as the first argument.
The second argument should be the name of the output file.
For Example:

```
JSBuilder main.js output.js
```

## Directives

Directives are instructions to JSBuilder that indicate how the output file should be built.
These directives are embedded in the JavaScript files using single-line comments.
For example:

```javascript
// directive: arg0 arg1 arg2 ...
```

#### Requires

The `requires` directive indicates that the given file is require before in order to interpret the proceeding code.
If the given file has not already been added to the output file, it will be added to the output.
If the file is already in the output, the directive will be ignored.
For example:

```javascript
// requires: car.js
var myCar = new Car();
```
