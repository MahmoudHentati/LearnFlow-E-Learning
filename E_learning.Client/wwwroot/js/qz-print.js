// QZ Print integration helper
console.log('qz-print.js loaded');

window.qzPrintHelper = {
    initialize: function() {
        console.log('QZ Print Helper initialized');
        return true;
    },
    findPrinters: function() {
        console.log('Finding available printers...');
        return [];
    },
    print: function(printerName, data) {
        console.log('Printing to:', printerName);
        return true;
    }
};
