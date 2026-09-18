// Print utility functions for QZ Tray integration
console.log('print.js loaded');

window.printUtilities = {
    print: function(config) {
        console.log('Print function called with config:', config);
        // Add print logic here
    },
    testConnection: function() {
        console.log('Testing print connection...');
        return true;
    }
};
