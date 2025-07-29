function generateCategories() {
    const today = new Date();
    const categories = [];

    for (let i = 5; i >= 0; i--) {
        const monthDate = new Date(today.getFullYear(), today.getMonth() - i, 1);
        categories.push(monthDate.toISOString());
    }

    return categories;
}



/*<sales chart>*/
    new Vue({
        el: '#appsales',
    components: {
        apexchart: VueApexCharts,
                        },
    data: {

        series: [{
        name: 'Sales',
    data: [90, 100, 19, 22, 9, 12, 7, 19, 5, 13, 9, 17]
                            }],
    chartOptions: {
        chart: {
            type: 'area',
            height: 160,
            sparkline: {
                enabled: true
            },
        },
        stroke: {
            curve: 'smooth'
        },
        
        yaxis: {
            min: 0
        },
        colors: ['#feb019'],
        opacity: 1,
        title: {
          
            offsetX: 0,
            style: {
                fontSize: '0px',
            }
        }        },


                        },

    })

   

/*<revenuew chart>*/
     

new Vue({
    el: '#apprevenue',
    components: {
        apexchart: VueApexCharts,
    },
    data: {

        series: [{
            name: 'TEAM A',
            type: 'column',
            data: [23, 11, 22, 27, 13, 22, 37, 21, 44, 22, 30],
          
        }, {
            name: 'TEAM B',
            type: 'area',
            data: [44, 55, 41, 67, 22, 43, 21, 41, 56, 27, 43],
          
        }, {
            name: 'TEAM C',
            type: 'line',
            data: [30, 25, 36, 30, 45, 35, 64, 52, 59, 36, 39],
         
        }],
        chartOptions: {
            chart: {
                height: 350,
                type: 'line',
                stacked: false,
                
            },
            stroke: {
                width: [0, 2, 5],
                curve: 'smooth',
                
               
            },
            plotOptions: {
                bar: {
                    columnWidth: '20%',
                    borderRadius: 4,
                    
                    
                }
            },
            colors: ['#a16efd', '#d3ebfc', '#febd43'],
            fill: {
                opacity: [0.85, 0.25, 1],
                gradient: {
                    inverseColors: false,
                    shade: 'light',
                    type: "vertical",
                    opacityFrom: 0.85,
                    opacityTo: 0.55,
                    stops: [0, 100, 100, 100]
                }
            },
            labels: ['01/01/2003', '02/01/2003', '03/01/2003', '04/01/2003', '05/01/2003', '06/01/2003', '07/01/2003',
                '08/01/2003', '09/01/2003', '10/01/2003', '11/01/2003'
            ],
            markers: {
                size: 0
            },
            xaxis: {
                type: 'datetime'
            },
            yaxis: {
                title: {
                    text: 'Points',
                },
                min: 0
            },
            tooltip: {
                shared: true,
                intersect: false,
                y: {
                    formatter: function (y) {
                        if (typeof y !== "undefined") {
                            return y.toFixed(0) + " points";
                        }
                        return y;

                    }
                }
            }
        },


    },

})



/*<order summury>*/
    //new Vue({
    //    el: '#appordersall',
    //    components: {
    //        apexchart: VueApexCharts,
    //    },
    //    data: {

    //        series: [13, 55, 17, 15],
    //        chartOptions: {
    //            chart: {
    //                type: 'donut',
    //                width: 380,

    //            }, plotOptions: {
    //                pie: {
    //                    donut: {
    //                        size: '89%',
    //                        labels: {
    //                            show: true,
    //                            total: {
    //                                showAlways: true,
    //                                show: true
    //                            }
    //                        }
    //                    }
    //                }
    //            },

    //            dataLabels: {
    //                enabled: false
    //            },

    //            legend: {
    //                show: false
    //            },
    //            colors: ['#febd43', '#27aae1', '#04df0c', '#ff4254'],
    //            responsive: [{
    //                breakpoint: 280,
    //                options: {
    //                    chart: {
    //                        width: 300
    //                    },
    //                    legend: {
    //                        position: 'bottom'
    //                    }
    //                }
    //            }]
    //        },


    //    },

    //})

function initializeChartOrderSummary(shipped, processing, delivered, completed, cancelled, returned) {
    new Vue({
        el: '#appordersall',
        components: {
            apexchart: VueApexCharts,
        },
        data: {
            series: [shipped, processing, delivered, completed, cancelled, returned], // Adjust values accordingly
            chartOptions: {
                chart: {
                    type: 'donut',
                    width: 380,
                },
                plotOptions: {
                    pie: {
                        donut: {
                            size: '89%',
                            labels: {
                                show: true,
                                total: {
                                    showAlways: true,
                                    show: true
                                }
                            }
                        }
                    }
                },
                dataLabels: {
                    enabled: false
                },
                legend: {
                    show: false
                },
                colors: ['#f4bb4e', '#27aae1', '#04df0c', '#05a03b', '#ff4254', '#4254ff'], // Add colors
                labels: ['Shipped', 'Processing', 'Delivered', 'Completed', 'Cancelled', 'Returned'], // Add labels
                responsive: [{
                    breakpoint: 280,
                    options: {
                        chart: {
                            width: 300
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            },
        },
    });
}



function initializeChartOrderSummaryBuyer(shipped, processing, delivered, completed, cancelled, returned) {
    new Vue({
        el: '#appordersallBuyer',
        components: {
            apexchart: VueApexCharts,
        },
        data: {
            series: [shipped, processing, delivered, completed, cancelled, returned], // Adjust values accordingly
            chartOptions: {
                chart: {
                    type: 'donut',
                    width: 380,
                },
                plotOptions: {
                    pie: {
                        donut: {
                            size: '89%',
                            labels: {
                                show: true,
                                total: {
                                    showAlways: true,
                                    show: true
                                }
                            }
                        }
                    }
                },
                dataLabels: {
                    enabled: false
                },
                legend: {
                    show: false
                },
                colors: ['#f4bb4e', '#27aae1', '#04df0c', '#df0c04', '#ff4254', '#4254ff'], // Add colors
                labels: ['Shipped', 'Processing', 'Delivered', 'Completed', 'Cancelled', 'Returned'], // Add labels
                responsive: [{
                    breakpoint: 280,
                    options: {
                        chart: {
                            width: 300
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            },
        },
    });
}
/*<credit chart>*/

//new Vue({
//    el: '#appcredit',
//    components: {
//        apexchart: VueApexCharts,
//    },
//    data: {

//        series: [{
//            name: 'series1',
//            data: [31, 40, 28, 51, 42, 109, 100]
//        }, {
//            name: 'series2',
//            data: [11, 32, 45, 32, 34, 52, 41]
//        }],
//        chartOptions: {
//            chart: {
//                height: 350,
//                type: 'area'
//            },
//            dataLabels: {
//                enabled: false
//            },
//            stroke: {
//                curve: 'smooth'
//            },
//            xaxis: {
//                type: 'datetime',
//                categories: ["2018-09-19T00:00:00.000Z", "2018-09-19T01:30:00.000Z", "2018-09-19T02:30:00.000Z", "2018-09-19T03:30:00.000Z", "2018-09-19T04:30:00.000Z", "2018-09-19T05:30:00.000Z", "2018-09-19T06:30:00.000Z"]
//            },
//            colors: ['#febd43', '#27aae1'],
//            tooltip: {
//                x: {
//                    format: 'dd/MM/yy HH:mm'
//                },
//            },
//        },


//    },

//})
function generateCategoriesMonth() {
    const today = new Date();
    const categories = [];

    for (let i = 4; i >= 0; i--) {
        const monthDate = new Date(today.getFullYear(), today.getMonth() - i, 1);
        const monthLabel = monthDate.toLocaleDateString('en-us', { year: 'numeric', month: 'short' });
        categories.push(monthLabel);
    }

    return categories;
}

function initializeChartCreditPurchased(dataListPurchase, dataListUsed) {
    // Parse the JSON data
    
    const dataArrayPurchased = JSON.parse(dataListPurchase);
    const dataArrayUsed = JSON.parse(dataListUsed);

    // Split the data into categories and series
    //const categories = dataArrayPurchased.map(item => item.split(': ')[0]);
    //const seriesDataPurchase = dataArrayPurchased.map(item => parseInt(item.split(': ')[1]));

    var months = generateCategoriesMonth();

    // Create dictionaries to store the data for each month
    const purchasedMonthData = {};
    dataArrayPurchased.forEach(item => {
        const parts = item.split(': ');
        const month = parts[0];
        const value = parseInt(parts[1]);
        purchasedMonthData[month] = value;
    });

    const usedMonthData = {};
    dataArrayUsed.forEach(item => {
        const parts = item.split(': ');
        const month = parts[0];
        const value = parseInt(parts[1]);
        usedMonthData[month] = value;
    });

    const purchasedDataForMonths = months.map(month => purchasedMonthData[month] || 0);
    const usedDataForMonths = months.map(month => usedMonthData[month] || 0);

    // Generate x-axis categories for the current year from January to September
    //const today = new Date();
    //const currentYear = today.getFullYear();
    //const categories = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'];
    //console.log(dataArrayPurchased);
    //console.log(seriesDataPurchase);
   /* console.log(seriesDataUsed);*/
    //console.log(dataArrayPurchased);
    //console.log(months);
    new Vue({
        el: '#appcreditPurchased',
        components: {
            apexchart: VueApexCharts,
        },
        data: {
            series: [
                {
                    name: "Purchased",
                    data: purchasedDataForMonths
                },
                {
                    name: "Used",
                    data: usedDataForMonths
                }
            ],
            chartOptions: {
                chart: {
                    height: 350,
                    type: 'line',
                    dropShadow: {
                        enabled: true,
                        color: '#000',
                        top: 18,
                        left: 7,
                        blur: 10,
                        opacity: 0.2
                    },
                    toolbar: {
                        show: false
                    }
                },
                colors: ['#007BFF', '#FFA500'],
                dataLabels: {
                    enabled: true,
                },
                stroke: {
                    curve: 'smooth'
                },
                title: {
                    text: 'Purchased & Used Credit',
                    align: 'left'
                },
                grid: {
                    borderColor: '#e7e7e7',
                    row: {
                        colors: ['#f3f3f3', 'transparent'],
                        opacity: 0.5
                    },
                },
                markers: {
                    size: 1
                },
                xaxis: {
                    categories: months,
                    title: {
                        text: 'Month'
                    }
                },
                yaxis: {
                    title: {
                        text: '# of Credits'
                    },
                    min: 0,
                    max: 1000
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'right',
                    floating: true,
                    offsetY: -25,
                    offsetX: -5
                }
            },
        },
    });
}



function initializeChartSubscription(dataList) {



    new Vue({
        el: '#appsubscription',
        components: {
            apexchart: VueApexCharts,
        },
        data: {
            series: [
                {
                    name: 'Subscription Package',
                    data: dataList
                }
            ],
            chartOptions: {
                chart: {
                    height: 350,
                    type: 'rangeBar'
                },
                plotOptions: {
                    bar: {
                        horizontal: true
                    }
                },
                xaxis: {
                    type: 'datetime'
                }
            },
        },
    });
}



/*    < Subscription >*/
    //new Vue({
    //    el: '#appsubscription',
    //    components: {
    //        apexchart: VueApexCharts,
    //    },
    //    data: {

    //        series: [{
    //            name: 'Subscription',
    //            data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
    //        }, {
    //            name: 'Expired',
    //            data: [6, 5, 11, 9, 7, 15, 9, 14, 9]
    //        }],
    //        chartOptions: {
    //            chart: {
    //                type: 'bar',
    //                height: 350
    //            },
    //            plotOptions: {
    //                bar: {
    //                    horizontal: false,
    //                    columnWidth: '55%',
    //                    endingShape: 'rounded'
    //                },
    //            },
    //            dataLabels: {
    //                enabled: false
    //            },
    //            stroke: {
    //                show: true,
    //                width: 2,
    //                colors: ['transparent']
    //            },
    //            xaxis: {
    //                categories: ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct'],
    //            },
    //            colors: ['#f9a402', '#f74051'],
    //            fill: {
    //                opacity: 1
    //            },
    //            tooltip: {
    //                y: {
    //                    formatter: function (val) {
    //                        return "$ " + val + " thousands"
    //                    }
    //                }
    //            }
    //        },


    //    },

    //})
 









/*    < order pending chart >*/

new Vue({
        el: '#ordersapppending',
        components: {
            apexchart: VueApexCharts,
        },
        data: {

            series: [20],
            colors: ["#000"],
            chartOptions: {
                chart: {
                    height: 350,
                    type: 'radialBar',
                    offsetY: 0
                },
                plotOptions: {
                    radialBar: {
                     
                        dataLabels: {
                            name: {
                                fontSize: '0px',
                                color: undefined,
                                offsetY: 0
                            },
                            value: {
                                offsetY: -10,
                                fontSize: '15px',
                                color: undefined,
                                formatter: function (val) {
                                    return val + "%";
                                }
                            }
                        }
                    }
                },
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'dark',
                        type: 'horizontal',
                        shadeIntensity: 0.5,
                        gradientToColors: ['#ffb626'],
                        inverseColors: false,
                        opacityFrom: 1,
                        opacityTo: 1,
                        stops: [0, 0]
                    }
                },
                stroke: {
                    dashArray: 2,
                   

                },
                labels: [' '],
            },


        },

    })
   /*    < order processing  chart >*/

new Vue({
        el: '#ordersappprocessing',
        components: {
            apexchart: VueApexCharts,
        },
        data: {

            series: [40],
            colors: ["#000"],
            chartOptions: {
                chart: {
                    height: 350,
                    type: 'radialBar',
                    offsetY: 0
                },
                plotOptions: {
                    radialBar: {
                     
                        dataLabels: {
                            name: {
                                fontSize: '0px',
                                color: undefined,
                                offsetY: 0
                            },
                            value: {
                                offsetY: -10,
                                fontSize: '15px',
                                color: undefined,
                                formatter: function (val) {
                                    return val + "%";
                                }
                            }
                        }
                    }
                },
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'dark',
                        type: 'horizontal',
                        shadeIntensity: 0.5,
                        gradientToColors: ['#27aae1'],
                        inverseColors: false,
                        opacityFrom: 1,
                        opacityTo: 1,
                        stops: [0, 0]
                    }
                },
                stroke: {
                    dashArray: 2,
                   

                },
                labels: [' '],
            },


        },

    })
    /*    < order delivered  chart >*/

new Vue({
        el: '#ordersappdelivered',
        components: {
            apexchart: VueApexCharts,
        },
        data: {

            series: [60],
            colors: ["#000"],
            chartOptions: {
                chart: {
                    height: 350,
                    type: 'radialBar',
                    offsetY: 0
                },
                plotOptions: {
                    radialBar: {
                     
                        dataLabels: {
                            name: {
                                fontSize: '20px',
                                color: undefined,
                                offsetY: 0
                            },
                            value: {
                                offsetY: -10,
                                fontSize: '15px',
                                color: undefined,
                                formatter: function (val) {
                                    return val + "%";
                                }
                            }
                        }
                    }
                },
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'dark',
                        type: 'horizontal',
                        shadeIntensity: 0.5,
                        gradientToColors: ['#00df08'],
                        inverseColors: false,
                        opacityFrom: 1,
                        opacityTo: 1,
                        stops: [0, 0]
                    }
                },
                stroke: {
                    dashArray: 2,
                   

                },
                labels: [' '],
            },


        },

    })


/*    < order Cancelled chart >*/
    new Vue({
        el: '#ordersappcancelled',
        components: {
            apexchart: VueApexCharts,
        },
        data: {

            series: [20],
            colors: ["#000"],
            chartOptions: {
                chart: {
                    height: 350,
                    type: 'radialBar',
                    offsetY: 0
                },
                plotOptions: {
                    radialBar: {
                     
                        dataLabels: {
                            name: {
                                fontSize: '20px',
                                color: undefined,
                                offsetY: 0
                            },
                            value: {
                                offsetY: -10,
                                fontSize: '15px',
                                color: undefined,
                                formatter: function (val) {
                                    return val + "%";
                                }
                            }
                        }
                    }
                },
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'dark',
                        type: 'horizontal',
                        shadeIntensity: 0.5,
                        gradientToColors: ['#dc3545'],
                        inverseColors: false,
                        opacityFrom: 1,
                        opacityTo: 1,
                        stops: [0, 0]
                    }
                },
                stroke: {
                    dashArray: 2,
                   

                },
                labels: [' '],
            },


        },

    })




/*    < seller dashboard order bar chart >*/
function sellerdashboardSalesSeggregation(process, shipped, delivered, completed, cancelled, returned, salescurrency) {


   


    new Vue({
        el: '#sellerdashboardorderbarchart',
        components: {
            apexchart: VueApexCharts,
        },
        data: {

            series: [{
                data: [process, shipped, delivered, completed, cancelled, returned]
            }],
            chartOptions: {
                chart: {
                    type: 'bar',
                    height: 380
                },
                plotOptions: {
                    bar: {
                        barHeight: '100%',
                        distributed: true,
                        horizontal: true,
                        dataLabels: {
                            position: 'bottom'
                        },
                    }
                },
                colors: ['#27aae1', '#eed92d', '#02d7b9', '#5acb65', '#ff4254', '#ff8708',

                ],
                dataLabels: {
                    enabled: true,
                    textAnchor: 'start',
                    style: {
                        colors: ['#fff']
                    },
                    formatter: function (val, opt) {
                        return opt.w.globals.labels[opt.dataPointIndex] + ":  " + val
                    },
                    offsetX: 0,
                    dropShadow: {
                        enabled: true
                    }
                },
                stroke: {
                    width: 1,
                    colors: ['#fff']
                },
                xaxis: {
                    categories: ['Processing ' + salescurrency, 'Shipped ' + salescurrency, 'Delivered ' + salescurrency, 'Completed ' + salescurrency, 'Cancelled ' + salescurrency, 'Returned ' + salescurrency],

                },
                yaxis: {
                    labels: {
                        show: false
                    }
                },
                title: {
                    text: 'Sales By Order Status' ,
                    align: 'center',
                    floating: true
                },
                subtitle: {
                    text: '',
                    align: 'center',
                },
                tooltip: {
                    theme: 'dark',
                    x: {
                        show: false
                    },
                    y: {
                        title: {
                            formatter: function () {
                                return ''
                            }
                        }
                    }
                }
            },


        },

    })

}


