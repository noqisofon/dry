const fs = require( 'fs' );

exports.command = 'init <name>';

exports.aliases = [ 'new' ];

exports.describe = 'initialize a Project';

exports.builder = {
    type: {
        alias: 't',
        describe: 'set project type'
    }
};

exports.handler = function (argv) {
    let project_name = argv.name;

    if ( !fs.existsSync ) {
        fs.mkdirSync( project_name );
    }
};
