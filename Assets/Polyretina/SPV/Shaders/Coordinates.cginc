#ifndef COORDINATES_CGINC
#define COORDINATES_CGINC

//
// visual angles (deg) <--> retinal distances (um)
//

	float angle_to_retina(float angle)
	{
		float pm = sign(angle);
		angle = abs(angle);
		float r_mm = 0.268 * angle + 3.427e-4 * pow(angle, 2.0) - 8.3309e-6 * pow(angle, 3.0);
		float r_um = 1e3 * r_mm;

		return pm * r_um;
	}

	float2 angle_to_retina(float2 angle)
	{
		return float2(angle_to_retina(angle.x), angle_to_retina(angle.y));
	}

	float retina_to_angle(float retina)
	{
		float pm = sign(retina);
		float r_mm = 1e-3 * abs(retina);
		float r_deg = 3.556 * r_mm + 0.05993 * pow(r_mm, 2.0) - 0.007358 * pow(r_mm, 3.0);
		r_deg += 3.027e-4 * pow(r_mm, 4.0);

		return pm * r_deg;
	}
	
	float2 retina_to_angle(float2 angle)
	{
		return float2(retina_to_angle(angle.x), retina_to_angle(angle.y));
	}
	


//
// retinal distances (um) <--> pixel coordinates (pix)
//

	float2 retina_to_pixel(float2 retina, float2 view_diameter)
	{
		return retina / view_diameter + 0.5;
	}

	float2 pixel_to_retina(float2 pixel, float2 view_diameter)
	{
		return (pixel - 0.5) * view_diameter;
	}



//
// visual angles (deg) <--> pixel coordinates (pix)
//

	float2 angle_to_pixel(float2 angle, float2 view_diameter)
	{
		float2 retina = angle_to_retina(angle);
		float2 pixel = retina_to_pixel(retina, view_diameter);
		return pixel;
	}
			
	float2 pixel_to_angle(float2 pixel, float2 view_diameter)
	{
		float2 retina = pixel_to_retina(pixel, view_diameter);
		float2 angle = retina_to_angle(retina);
		return angle;
	}

	

//
// polar (rho, phi) <--> visual angles (deg)
//

	float2 polar_to_angle(float rho, float phi)
	{
		float2 angle = float2(0.0, 0.0);

		float prime = rho * cos(radians(phi));

		angle.x = prime + 15.0;
		angle.y = rho * sin(radians(phi));

		if (prime > -15.0)
		{
			angle.y += 2.0 * pow(angle.x / 15.0, 2.0);
		}

		return angle;
	}
	


//
// polar (rho, phi) <--> pixel coordiantes (pix)
//

	float2 polar_to_pixel(float rho, float phi, float2 view_diameter)
	{
		float2 angle = polar_to_angle(rho, phi);
		float2 pixel = angle_to_pixel(angle, view_diameter);
		return pixel;
	}
	
#endif
