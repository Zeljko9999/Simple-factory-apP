const Base_URL = 'http://localhost:5027'

class _CarAPI { 

    async GetAllCars() {
        const URL = `${Base_URL}/api/Car`;
        const response = await fetch(URL, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if(!response.ok) {
            console.error('Could not get cars from the API!')
            return null;
        }

        return response.json();
    }

    // Returns true if successful and false if failed
    async CreateNewCar(car) {
        console.log(JSON.stringify(car))
        console.log(car)
        const URL = `${Base_URL}/api/Car`;
        const response = await fetch(URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(car)
            
        });

        if(!response.ok) {
            console.error('Could not create new car.')
            if(response.status === 400) { /* Bad Request */
                alert(await response.text())
            }
            return false;
        }

        return true;
    }

    async DeleteCar(carId) {
        const URL = `${Base_URL}/api/Car/${carId}`;
        const response = await fetch(URL, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if(!response.ok) {
            console.error(`Could not delete car with id = ${carId}.`)
            if(response.status === 400) { /* Bad Request */
                alert(await response.text())
            }
        }
    }

}

export const CarAPI = new _CarAPI();