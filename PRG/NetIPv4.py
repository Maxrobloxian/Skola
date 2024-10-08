# Jsem kod co testuje IPv4 adresy, mam 13 validnych a 7 nevalidnych ip adres
# Vsechny je zkotroluju a zapisuji
# Pokud nutno ti nopisu i hybu

import random
from colorama import Fore

class AddressIPv4:
    def __init__(self, ip):
        if self.isValid(ip): self.address = self.__saveIP(ip)
        else: raise Exception("Invalid address")

    def isValid(self, ip):
        if type(ip) != str: raise Exception("Invalid type")
        tempAddress = ip.split(".")
        if len(tempAddress) != 4: raise Exception("Invalid length")
        for i in tempAddress:
            if (int(i) < 0 or int(i) > 255):raise Exception("Invalid address size")
        return True

    def __saveIP(self, ip):
        address = 0
        multiplier = 24
        for i in ip.split("."):
            address += int(i)*2**multiplier
            multiplier -= 8
        return address
    
    def __getAsList(self):
        x = self.address
        y = 24
        listAddress = []
        for i in range(4):
            listAddress.append(str(x//(2**y)))
            x %= 2**y
            y -= 8
        return listAddress

    def set(self, ip):
        if self.isValid(ip): self.address = self.__saveIP(ip)
        else: raise Exception("Invalid IP")
    
    def getAsString(self):
        x = self.__getAsList()
        stringAddress = ""
        for i in range(4):
            if i != 0: stringAddress += "."
            stringAddress += x[i]
        return stringAddress

    def getAsInt(self):
        return self.address
    
    def getAsBinaryString(self):
        x = self.__getAsList()
        y = ""
        binarAddress = ""
        for i in x:
            i = int(i)
            if i == 0: binarAddress += "0"
            while i > 0:
                y += str(i & 1)
                i //= 2
            binarAddress += y[::-1] + "."
            y = ""
        return binarAddress[:-1]
    
    def getOctet(self, octetNum):
        if octetNum < 0 or octetNum > 3: raise Exception("Invalid octet number")
        return int(self.__getAsList()[octetNum])

    def getClass(self):
        x = int(self.__getAsList()[0])
        if (x >= 0 and x <= 127): return "A"
        elif (x >=128 and x <= 191): return "B"
        elif (x >= 192 and x <= 223): return "C"
        elif (x >= 224 and x <= 239): return "D"
        else: return "E"

    def isPrivate(self):
        x = self.__getAsList()
        if (int(x[0]) == 10): return True
        elif (int(x[0]) == 172 and int(x[1]) >= 16 and int(x[1]) <= 31): return True
        elif (int(x[0]) == 192 and int(x[1]) == 168): return True
        else: return False

# ---------------------------------------------TEST ZONE---------------------------------------------
# Valid addresses
addresses = []
addresses.append(AddressIPv4("192.168.5.6"))
addresses.append(AddressIPv4("10.6.26.72"))
addresses.append(AddressIPv4("172.27.84.31"))
addresses.append(AddressIPv4("192.168.8.3"))
addresses.append(AddressIPv4("10.9.35.1"))
addresses.append(AddressIPv4("172.30.224.224"))
addresses.append(AddressIPv4("192.168.11.246"))
addresses.append(AddressIPv4("10.12.63.52"))
setAddresses = ["172.34.119.98", "192.168.15.128", "10.17.0.4", "172.40.0.1", "192.168.22.9"]

for i in range(len(addresses)):
    print(Fore.LIGHTBLUE_EX + addresses[i].getAsString().center(75, "-") + Fore.RESET)
    if random.randint(0, 10) <= 3:
        addresses[i].set(setAddresses[random.randint(0, len(setAddresses) - 1)])
        print(f"{Fore.LIGHTGREEN_EX}Changed the address to {addresses[i].getAsString()}{Fore.RESET}")
    print(f"GetAsInt: {addresses[i].getAsInt()}")
    print(f"GetAsString: {addresses[i].getAsString()}")
    print(f"GetAsBinaryString: {addresses[i].getAsBinaryString()}")
    print(f"GetOctet: {addresses[i].getOctet(0)}")
    print(f"GetClass: {addresses[i].getClass()}")
    print(f"IsPrivate: {addresses[i].isPrivate()}")

# ---------------------------------------------------------------------------------------------------
# Invalid addresses    
invalidAddresses = ["1942.168.15.1", "10.17.0-1.1", "172.-46.0.1", "192.168.284.1", "10..0.1", "10.1", "172.50.0.1.141.21.234"]
for i in range(len(invalidAddresses)):
    try:
        addresses[i].set(invalidAddresses[i])
    except Exception as e:
        print(Fore.RED + "\n" + str(e) + Fore.RESET)
