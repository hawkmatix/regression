Hawkmatix Regression
====================

Regression is the process of determining a best fit curve through a data set.
This project uses quadratic polynomial regression to determine a polynomial
that fits the data best. That polynomial is then solved for at the current time
and plotted. This yields a close fit to the data with very high correlation.
Extrapolations can be drawn from the polynomial that supports future price
predictions. A visual aid was built into this project allowing a color change
when either the value is increasing or decreasing or the line acts as support
or resistance. 

Installation
------------

Install from source::

    

Package Contents
----------------

    moving-median
        Regression sources.

Usage
-----

This software is intended for use with the NinjaTrader trading platform.
Full documentation is available at
http://Hawkmatix.github.io/regression.html

Supported Operating Environment
-------------------------------

This version of the add-on software has been tested, and is known to work
against the following NinjaTrader versions and operating systems.

NinjaTrader Versions
~~~~~~~~~~~~~~~~~~~~

* NinjaTrader 7.0.1000.27
* NinjaTrader 6.5.1000.19

Operating Systems
~~~~~~~~~~~~~~~~~

* Windows 7/8

Requirements
------------

Supports NinjaTrader 6.5.1000.19 - 7.0.1000.27.

License
-------

All code contained in this repository is Copyright 2012-Present Andrew C.
Hawkins.

This code is released under the GNU Lesser General Public License. Please see
the COPYING and COPYING.LESSER files for more details.

Contributors
------------

* Andrew C. Hawkins <andrew@hawkmatix.com>

Changelog
---------

* v2 Visual aids are added in the form of changing colors. They can be applied
when the value is increasing or decreasing or acts as support or resistance.

* v1 Standard calculations for cubic polynomial interpolation are added and the
value of the polynomial found is plotted at the endpoint, or current time.
