import type { CarouselProps } from "react-multi-carousel";
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";

interface CustomSliderProps extends React.HTMLAttributes<HTMLDivElement> {
  carouselProps: CarouselProps;
}

export const CustomSlider: React.FC<CustomSliderProps> = ({
  carouselProps: { children, responsive, ...caroProps },
  style,
  ...props
}) => {
  return (
    <div
      style={{
        position: "relative",
        width: "calc(100vw - 260px - 48px)",
        ...style,
      }}
      {...props}
    >
      <Carousel responsive={responsive} {...caroProps}>
        {children}
      </Carousel>
    </div>
  );
};
