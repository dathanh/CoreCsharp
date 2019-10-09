const config = require('config');
const dirConfig = config.get('Dir');
const projectName = config.get('ProjectName');
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
const projectDir = __dirname.replaceAll('\\', '/').replace('/generate', '');
let formatDir = [];
for (var dir in dirConfig) {
    if (dir == 'CoreModule' || dir == 'WebModule' || dir == 'Controller'
        || dir == 'Mapping' || dir == 'Models' || dir == 'Views'
        || dir == 'Router' || dir == 'Main' || dir == 'ControllerJs') {
        formatDir[dir] = projectDir + `/${projectName}` + dirConfig[dir];
    } else {
        formatDir[dir] = projectDir + dirConfig[dir];
    }
}
module.exports = formatDir;