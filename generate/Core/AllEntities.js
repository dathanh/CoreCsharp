const fs = require('fs');
const config = require('config');
const ignoreFile = config.get('IgnoreFile');
let allEntity = []
const genDir = __dirname.replace('generate\\Core', 'generate\\').replaceAll('\\', '/');
const entityFiles = fs.readdirSync(genDir + '/Database/');

//get all entity name;
entityFiles.forEach(file => {
    entityName = file.replace('.js', '')
    if (ignoreFile.indexOf(entityName) < 0) {
        allEntity.push(entityName);
    }
});

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
module.exports = allEntity;