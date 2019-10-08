const fs = require('fs');
const Handlebars = require('handlebars');
const exec = require('child_process').exec;
const dir = require('./dir');
const source = '<div>{{title}}</div>';

console.log(dir);
// exec("node ./Command/CreateEntity create customer",function(data){
//     console.log(dbConfig);
// });

